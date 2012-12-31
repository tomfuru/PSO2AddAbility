using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO2AddAbility
{
    /// <summary>武器を作るための(複数の)合成情報</summary>
    public class WeaponSynthesisInfo
    {
        public double Cost;
        public Weapon Weapon;
        public SynthesisWeapons[] SynthesisInfo;
    }

    /// <summary>一つの合成の情報</summary>
    public class SynthesisWeapons
    {
        public double cost;
        public float[] probabilities;
        public WeaponSynthesisInfo info0;
        public WeaponSynthesisInfo info1;
        public WeaponSynthesisInfo info2;
    }

    public class Synthesis
    {
        //-------------------------------------------------------------------------------
        #region Constants
        //-------------------------------------------------------------------------------
        private static readonly int[][][][] COMBINATION = {
           null,null,
            new int[][][] {
                null,
                new int[][]{ new []{0}, new []{1}},
                new int[][]{ new []{0, 1}}
            },
            new int[][][] {
                null,
                new int[][]{ new []{0}, new []{1}, new [] {2}},
                new int[][]{ new []{0, 1}, new []{1, 2}, new []{2,0}},
                new int[][]{ new []{0, 1, 2}}
            }
        };
        //-------------------------------------------------------------------------------
        #endregion (Constants)

        private Synthesis() { }

        //-------------------------------------------------------------------------------
        #region (Class)SynthesisGeneralInfo
        //-------------------------------------------------------------------------------
        public class SynthesisGeneralInfo
        {
            public int Material_Synthesis_2weapons { get; private set; }
            public int Material_Synthesis_3weapons { get; private set; }
            public int Synthesis_2weapons { get; private set; }
            public int Synthesis_3weapons { get; private set; }
            public ValueData ValueData { get; private set; }

            //-------------------------------------------------------------------------------
            #region Constructor
            //-------------------------------------------------------------------------------
            public SynthesisGeneralInfo(SettingsData settings, int synthesis_2, int synthesis_3)
            {
                this.Material_Synthesis_2weapons = settings.Material_Synthesis_2weapons;
                this.Material_Synthesis_3weapons = settings.Material_Synthesis_3weapons;
                this.ValueData = settings.ValueData;

                this.Synthesis_2weapons = synthesis_2;
                this.Synthesis_3weapons = synthesis_3;
            }
            //-------------------------------------------------------------------------------
            #endregion (Constructor)
        }
        //-------------------------------------------------------------------------------
        #endregion (SynthesisGeneralInfo)

        //-------------------------------------------------------------------------------
        #region  -(class)AbilityInfo
        //-------------------------------------------------------------------------------
        private class AbilityInfo
        {
            public IAbility Ability;
            public int Num;
            public override string ToString()
            {
                if (Ability == null) { return base.ToString(); }
                return string.Format("{0} * {1}", Ability.ToString(), Num);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ( -(class)AbilityInfo)

        /// <summary>メモ化用キャッシュ</summary>
        //private static Dictionary<Weapon, SynthesisWeapons[]> winfo_cache = new Dictionary<Weapon, SynthesisWeapons[]>();
        private static ConcurrentDictionary<Weapon, SynthesisWeapons[]> winfo_cache = new ConcurrentDictionary<Weapon, SynthesisWeapons[]>();
        private static ConcurrentDictionary<Weapon, SynthesisWeapons[]> winfo_cache_material = new ConcurrentDictionary<Weapon, SynthesisWeapons[]>();
        //-------------------------------------------------------------------------------
        #region +Synthesize
        //-------------------------------------------------------------------------------
        //
        public static SynthesisWeapons[] Synthesize(Weapon objective, bool isParallel, SynthesisGeneralInfo synInfo, Action<int> reportCombNum, Action reportFinOneComb)
        {
            return SynthesizeInternal(objective, false, isParallel, synInfo, reportCombNum, reportFinOneComb, true);
        }
        #endregion (Synthesize)
        //-------------------------------------------------------------------------------
        #region -SynthesizeInternal
        //-------------------------------------------------------------------------------
        //
        private static SynthesisWeapons[] SynthesizeInternal(Weapon objective, bool isMaterial, bool isParallel, SynthesisGeneralInfo synInfo, Action<int> reportCombNum, Action reportFinOneComb, bool isTopLevel)
        {
            if (IsBasicWeapon(objective, isMaterial)) { // BasicWeaponならそれ以上探さない
                return null;
            }

            if (isMaterial) {
                // 素材武器のみ
                if (winfo_cache_material.ContainsKey(objective)) { // 今まであったものなら参照
                    return winfo_cache_material[objective];
                }
            }
            else {
                // 素材武器でないメイン武器
                if (winfo_cache.ContainsKey(objective)) { // 今まであったものなら参照
                    return winfo_cache[objective];
                }
                else if (objective.All(iab => iab is ゴミ)) { // ゴミだけならそれに特化したメソッド
                    return SynthesizeToIncreaseSlotNum(objective, synInfo);
                }
            }
            int num_slot = objective.AbilityNum;
            // 必要な素材能力の候補リストを取得
            var need_materials = objective.Where(iab => !(iab is ゴミ)).Select(Data.GetMaterialAbilities).ToArray();

            // 1能力に必要な能力が複数ある(パワーⅢはパワーⅡ*2でも3でも作れる等)ため，それらの候補のすべての組み合わせリストを取得
            // 候補->全能力[全能力->1能力の必要能力[1能力の必要能力->能力]]
            List<IEnumerable<IEnumerable<IAbility>>> combinations = ListCombinations(need_materials);

            if (isTopLevel && reportCombNum != null) reportCombNum(combinations.Count);

            List<SynthesisWeapons> sw_list = new List<SynthesisWeapons>();
            // 各素材の組み合わせについて合成方法を調べる
            foreach (var combination in combinations) {
                List<AbilityInfo> elements = new List<AbilityInfo>(); // 必要特殊能力リスト
                foreach (var elem in combination) {
                    IAbility[] elemarray = elem.ToArray();
                    IAbility[] distinct = elem.Distinct().ToArray();
                    foreach (var element in distinct) {
                        //int num = elemarray.Count(el => el.GetType().Equals(element.GetType()));
                        //int index = elements.FindIndex(ai => ai.Ability.GetType().Equals(element.GetType()));
                        int num = elemarray.Count(el => el == element);
                        int index = elements.FindIndex(ai => ai.Ability == element);

                        // 既に入っている場合はその分入れなくてよい(例:アビリティⅢの素材のパワーⅢと、パワーⅢの素材のパワーⅢは重複可能)
                        if (index >= 0) {
                            elements[index].Num = Math.Max(elements[index].Num, num);
                        }
                        else {
                            elements.Add(new AbilityInfo() { Ability = element, Num = num });
                        }
                    }
                }
                elements.Sort((el1, el2) => -el1.Num.CompareTo(el2.Num));
                int num_elements = elements.Sum(ai => ai.Num);

                /*
                var a1 = Assignments(3, num_slot, elements).ToArray();
                var a2 = Assignments(3, num_slot - 1, elements).ToArray();
                var a3 = Assignments(2, num_slot, elements).ToArray();
                var a4 = Assignments(2, num_slot - 1, elements).ToArray();
                */

                Func<Weapon[], SynthesisWeapons> synthesis = combweapon =>
                {
                    bool three_weapons = (combweapon.Length == 3);
                    var probs = GetProbabilities(objective, combweapon[0], combweapon[1], (three_weapons) ? combweapon[2] : null); // 確率を求める

                    WeaponSynthesisInfo info0 = new WeaponSynthesisInfo() {
                        Weapon = combweapon[0],
                        SynthesisInfo = (IsBasicWeapon(combweapon[0], isMaterial)) ? null : SynthesizeInternal(combweapon[0], isMaterial, false, synInfo, reportCombNum, reportFinOneComb, false)
                    };
                    info0.Cost = GetCostOfWeapon(info0.Weapon, info0.SynthesisInfo, synInfo);
                    WeaponSynthesisInfo info1 = new WeaponSynthesisInfo() {
                        Weapon = combweapon[1],
                        SynthesisInfo = (IsBasicWeapon(combweapon[1], false)) ? null : SynthesizeInternal(combweapon[1], true, false, synInfo, reportCombNum, reportFinOneComb, false)
                    };
                    info1.Cost = GetCostOfWeapon(info1.Weapon, info1.SynthesisInfo, synInfo);
                    WeaponSynthesisInfo info2 = (three_weapons) ? new WeaponSynthesisInfo() {
                        Weapon = combweapon[2],
                        SynthesisInfo = (IsBasicWeapon(combweapon[2], false)) ? null : SynthesizeInternal(combweapon[2], true, false, synInfo, reportCombNum, reportFinOneComb, false)
                    }
                    : null;
                    if (info2 != null) { info2.Cost = GetCostOfWeapon(info2.Weapon, info2.SynthesisInfo, synInfo); }

                    double cost_weapons = info0.Cost + info1.Cost + ((info2 == null) ? 0.0d : info2.Cost);
                    int cost_synthesis = (info2 == null) ? (isMaterial ? synInfo.Material_Synthesis_2weapons : synInfo.Synthesis_2weapons)
                                                         : (isMaterial ? synInfo.Material_Synthesis_3weapons : synInfo.Synthesis_3weapons);

                    SynthesisWeapons sw = new SynthesisWeapons() {
                        info0 = info0,
                        info1 = info1,
                        info2 = info2,
                        probabilities = probs,
                        cost = (cost_weapons + cost_synthesis) / probs.Aggregate(1.0f, (f1, f2) => f1 * f2)
                    };

                    return sw;
                };

                Action<Weapon[]> synthesisComb = (co) =>
                {
                    if (objective.Equals(co[0])) { return; } // 目標武器に目標能力がついていたら意味がない
                    if (isMaterial && (objective.Equals(co[1]) || (co.Length > 2 && objective.Equals(co[2])))) { return; } // 素材を作成している場合素材にその能力がついていたら意味がない

                    var sw = synthesis(co);
                    sw_list.Add(sw);
                };

                var assinedComb = Assignments(3, num_slot, elements)            // 3本，目的能力数同士の合成
                              .Concat(Assignments(3, num_slot - 1, elements))   // 3本，目的能力数-1同士の合成
                              .Concat(Assignments(2, num_slot, elements))       // 2本，目的能力数同士の合成
                              .Concat(Assignments(2, num_slot - 1, elements))   // 2本，目的能力数-1同士の合成
                              .ToArray();

                if (isParallel) { // パラレル
                    Parallel.ForEach(assinedComb, synthesisComb);
                }
                else { // シーケンシャル
                    foreach (var co in assinedComb) {
                        synthesisComb(co);
                    }
                }

                if (isTopLevel && reportFinOneComb != null) reportFinOneComb();
            }

            SynthesisWeapons[] ret = sw_list.ToArray();

            var dic = (isMaterial) ? winfo_cache_material : winfo_cache;
            if (!dic.ContainsKey(objective)) {
                dic.TryAdd(objective, ret);
            }

            return ret;
        }
        #endregion (SynthesizeInternal)
        //-------------------------------------------------------------------------------
        #region -SynthesizeToIncreaseSlotNum
        //-------------------------------------------------------------------------------
        //
        private static SynthesisWeapons[] SynthesizeToIncreaseSlotNum(Weapon objective, SynthesisGeneralInfo synInfo)
        {
            if (objective.AbilityNum == 0) { return null; } 

            Debug.Assert(objective.All(iab => iab is ゴミ));

            bool three = (objective.AbilityNum >= 2);

            Weapon weapon = new Weapon(Enumerable.Repeat(ゴミ.Get(), objective.AbilityNum - 1).ToArray());

            WeaponSynthesisInfo info = new WeaponSynthesisInfo() {
                Weapon = weapon,
                SynthesisInfo = SynthesizeToIncreaseSlotNum(weapon, synInfo),
            };
            info.Cost = GetCostOfWeapon(info.Weapon, info.SynthesisInfo, synInfo);

            Weapon weaponM = (three) ? weapon : new Weapon(ゴミ.Get());
            WeaponSynthesisInfo infoM = new WeaponSynthesisInfo() {
                Weapon = weaponM,
                SynthesisInfo = null
            };
            infoM.Cost = GetCostOfWeapon(infoM.Weapon, infoM.SynthesisInfo, synInfo);

            float[] probabilities = GetProbabilities(objective, weapon, weaponM, (three) ? weaponM : null);
            double cost = ((three) ? (info.Cost + infoM.Cost * 2 + synInfo.Synthesis_3weapons) : (info.Cost + infoM.Cost + synInfo.Synthesis_2weapons)) / Util.AllProbability(probabilities);

            return new SynthesisWeapons[] {
                new SynthesisWeapons() { 
                        info0 = info,
                        info1 = infoM,
                        info2 = (three) ? infoM : null,
                        probabilities = probabilities,
                        cost = cost
                }
            };
        }
        #endregion (SynthesizeToIncreaseSlotNum)
        //-------------------------------------------------------------------------------
        #region -GetCostOfWeapon
        //-------------------------------------------------------------------------------
        //
        private static double GetCostOfWeapon(Weapon weapon, SynthesisWeapons[] synweps, SynthesisGeneralInfo synInfo)
        {
            if (synweps != null) {
                return synweps.Where(swp => !Double.IsNaN(swp.cost))
                                            .Min(swp => swp.cost);
            }
            else {
                IAbility ab = weapon.FirstOrDefault(iab => !(iab is ゴミ));
                int ability_num = weapon.AbilityNum;
                if (ab == null) {
                    return synInfo.ValueData.GarbageValues[ability_num];
                }
                else {
                    SerializableTuple<AbilityType, int> tuple;
                    if (ab is ILevel) {
                        ILevel ab_lv = ab as ILevel;
                        tuple = SerializableTuple.Create(Data.DIC_IABILITY_TO_ABILITYTYPE[ab_lv.GetInstanceOfLv(1)], ab_lv.Level);
                    }
                    else {
                        tuple = SerializableTuple.Create(Data.DIC_IABILITY_TO_ABILITYTYPE[ab], 0);
                    }

                    return (synInfo.ValueData.ValueDataDic.ContainsKey(tuple))
                            ? synInfo.ValueData.ValueDataDic[tuple][ability_num]
                            : Double.NaN;
                }
            }
        }
        #endregion (GetCostOfWeapon)

        //-------------------------------------------------------------------------------
        #region -Assignments 特殊能力の振り分けリスト
        //-------------------------------------------------------------------------------
        //
        private static IEnumerable<Weapon[]> Assignments(int weapon_num, int slot_num, List<AbilityInfo> elements)
        {
            if (weapon_num != 2 && weapon_num != 3) { throw new NotSupportedException("武器の個数は2個か3個のみ"); }

            if (weapon_num * slot_num < elements.Sum(ai => ai.Num)) { yield break; }  // 必要特殊能力数が武器数*スロット数より多いなら不可能
            if (elements.Max(ai => ai.Num) > weapon_num) { yield break; }             // 同じ特殊能力が武器の個数より多く必要なら不可能

            int[][][] combinations = COMBINATION[weapon_num];

            // { {{0},{1}}, {{0,1}} } => { {{0}, {0,1}}, {{1}, {0,1}} } の左辺部分(それぞれの属性のつき方のありうる組み合わせの集合)
            var elementsNums = elements.Select(ai => combinations[ai.Num]).ToArray();
            // すべての属性の付き方の組み合わせの列挙
            var list = ListCombinations(elementsNums);
            // 要素インデックス(引数のリストの)に関連付け
            var elemIndicieslists = list.Select(combination => Enumerable.Range(0, elements.Count).Zip(combination, (index, comb) => Tuple.Create(index, comb)));
            // (要素インデックス->武器インデックスリスト)のリストを(武器インデックス->要素インデックスリスト)のリストに
            var weapon_elements_list = elemIndicieslists.Select(eilist => Enumerable.Range(0, weapon_num).Select(i => eilist.Where(tuple => tuple.Item2.Contains(i)).Select(tuple => tuple.Item1).ToArray()).ToArray());
            // 2本目と3本目は区別しない
            var weapon_elements_list_distinct =
                (weapon_num == 2) ? weapon_elements_list :
                                    weapon_elements_list.Distinct((left, right) => left[1].Length == right[2].Length && left[1].All(i => right[2].Contains(i))
                                                                                && right[1].Length == left[2].Length && right[1].All(i => left[2].Contains(i)));
            // 一つの武器への割り当てが多すぎるものを排除
            var weapon_elements_list_m = weapon_elements_list_distinct.Where(we => we.All(weaponElems => weaponElems.Length <= slot_num));

            var weaponslist = weapon_elements_list_m.Select(indicieslist =>
                                   indicieslist.Select(indicies =>
                                       new Weapon(indicies.Select(index => elements[index].Ability)
                                                               .Concat(Enumerable.Repeat(ゴミ.Get(), slot_num - indicies.Length)).ToArray())
                                    ).ToArray()
                                );

            foreach (var item in weaponslist) { yield return item; }
        }
        #endregion (Assignments)

        //-------------------------------------------------------------------------------
        #region +IsBasicWeapon
        //-------------------------------------------------------------------------------
        //
        public static bool IsBasicWeapon(Weapon weapon, bool isMaterial)
        {
            if (isMaterial) {
                // Material weapon
                int garbage_num = weapon.Count(ab => ab is ゴミ);
                return (garbage_num >= weapon.AbilityNum - 1);
            }
            else {
                // Main weapon
                return (weapon.AbilityNum == 0);
            }
        }
        #endregion (IsBasicWeapon)

        //-------------------------------------------------------------------------------
        #region -ListCombinations 組み合わせを列挙
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 組み合わせ列挙，すべての要素の積の数のリストが返る
        /// {{1,2,3}, {4}, {5,6}} => {{1,4,5}, {1,4,6}, {2,4,5}, {2,4,6}, {3,4,5}, {3,4,6}}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        private static List<IEnumerable<T>> ListCombinations<T>(IEnumerable<T>[] values)
        {
            List<IEnumerable<T>> combinations = new List<IEnumerable<T>>();
            // 組み合わせを列挙
            Action<int, IEnumerable<T>> act = null;
            act = (i, tmplist) =>
            {
                foreach (var vals in values[i]) {
                    var concatted = tmplist.Concat(new T[] { vals });
                    if (values.Length - 1 == i) {
                        combinations.Add(concatted);
                    }
                    else {
                        act(i + 1, concatted);
                    }
                }
            };
            act(0, new T[] { });

            return combinations;
        }
        #endregion (ListCombinations)

        //-------------------------------------------------------------------------------
        #region +GetProbabilities
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 全能力についての能力付加確率を返す(EXTRA補正込み)
        /// </summary>
        /// <returns></returns>
        public static float[] GetProbabilities(Weapon objective, Weapon weapon1, Weapon weapon2, Weapon weapon3 = null)
        {
            int num_material_obj = objective.AbilityNum;
            int num_material_mat = weapon1.AbilityNum;
            int weapon_num = (weapon3 == null) ? 2 : 3;
            float correction_extra = (num_material_obj == num_material_mat) ? 1.00f : Data.PROB_CORRECTION_EXTRA[num_material_mat, weapon_num];

            return objective.Select(ab => GetProbability(ab, weapon1, weapon2, weapon3) * correction_extra).ToArray();
        }
        #endregion (GetProbabilities)
        //-------------------------------------------------------------------------------
        #region +GetProbability
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>[a,b]</para>
        /// <para>a:能力UPの時0,効果付加の時1</para>
        /// <para>b:ミューテーションⅠ付2, 増幅対象ソール付1,その他0</para>
        /// </summary>
        private static readonly float[,][,] PROB_TABLE = {
            {Data.PROB_NORMAL_BASIC, Data.PROB_SOUL_BASIC, Data.PROB_MUTATION1_BASIC},
            {Data.PROB_NORMAL_ADDITIONAL, Data.PROB_SOUL_ADDITIONAL, Data.PROB_MUTATION1_ADDITIONAL}
        };
        /// <summary>
        /// 1能力についての能力付加確率を返す(EXTRA補正無し)
        /// </summary>
        /// <returns></returns>
        public static float GetProbability(IAbility objective, Weapon weapon1, Weapon weapon2, Weapon weapon3 = null)
        {
            IEnumerable<Weapon> weapons = (new Weapon[] { weapon1, weapon2, weapon3 }).Where(w => w != null);
            if (objective is ゴミ) { return 1.00f; }
            if (objective is Boost) { return 1.00f; }
            if (objective is Soul || objective is Special_up) {
                int num = weapons.Count(weapon => weapon.ContainsAbility(objective));
                return Data.PROB_SOUL[Data.INH[num]];
            }
            else if (objective is Ability) {
                ILevel ab_lv = objective as ILevel;
                int inh_num = weapons.Count(weapon => weapon.ContainsAbility(objective));
                float inh_prob = (inh_num >= 1) ? Data.PROB_NORMAL_ABILITY[ab_lv.Level, Data.INH[inh_num]] : 0.0f;


                bool generable = weapons.Any(weapon => weapon.ContainsAbility(パワー.GetLv(ab_lv.Level)))
                              && weapons.Any(weapon => weapon.ContainsAbility(シュート.GetLv(ab_lv.Level)))
                              && weapons.Any(weapon => weapon.ContainsAbility(テクニック.GetLv(ab_lv.Level)));
                float gen_prob = (generable) ? Data.PROB_NORMAL_ABILITY[ab_lv.Level, Data.GEN_SP] : 0.0f;

                return Math.Max(inh_prob, gen_prob);
            }
            else if (objective is Basic_up || objective is Additional) {
                ILevel ab_lv = objective as ILevel;
                bool mutation_amp = (objective is IMutationAmplifiable) && weapons.Any(weapon => weapon.ContainsAbility(ミューテーション.GetLv(1)));
                bool soul_amp = !mutation_amp && weapons.Any(weapon => weapon.Any(ab => ab is Soul && (ab as Soul).IsAmplifiableAbility(objective))); // !mutation_ampはミューテーションによる増加がある場合はソールについて調べる必要がない為

                float[,] prob_table = PROB_TABLE[(objective is Basic_up) ? 0 : 1, (mutation_amp) ? 2 : (soul_amp) ? 1 : 0];

                int inh_num = weapons.Count(weapon => weapon.ContainsAbility(objective));
                float inh_prob = (inh_num >= 1) ? prob_table[ab_lv.Level, Data.INH[inh_num]] : 0.0f;

                IAbility gen_ab = ab_lv.GetInstanceOfLv(ab_lv.Level - 1);
                int gen_num = weapons.Count(weapon => weapon.ContainsAbility(gen_ab));
                float gen_prob = (gen_num >= 2) ? prob_table[ab_lv.Level - 1, Data.GEN[gen_num]] : 0.0f;

                return Math.Max(inh_prob, gen_prob);
            }

            Debug.Assert(false);
            return 0.0f;
        }
        #endregion (GetProbability)

    }
}
