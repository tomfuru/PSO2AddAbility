using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{
    public class WeaponSynthesisInfo
    {
        public Weapon Weapon;
        public SynthesisWeapons[] SynthesisInfo;
    }

    public class SynthesisWeapons
    {
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
        private static Dictionary<Weapon, SynthesisWeapons[]> winfo_cache = new Dictionary<Weapon, SynthesisWeapons[]>();
        //-------------------------------------------------------------------------------
        #region +Synthesize
        //-------------------------------------------------------------------------------
        //
        public static SynthesisWeapons[] Synthesize(Weapon objective, bool isMaterial)
        {
            if (winfo_cache.ContainsKey(objective)) { // 今まであったものなら参照
                return winfo_cache[objective];
            }
            else if (IsBasicWeapon(objective)) { // BasicWeaponならそれ以上探さない
                return null;
            }

            int num_slot = objective.AbilityNum;
            // 必要な素材能力の候補リストを取得
            var need_materials = objective.Select(Data.GetMaterialAbilities).ToArray();

            // 1能力に必要な能力が複数ある(パワーⅢはパワーⅡ*2でも3でも作れる等)ため，それらの候補のすべての組み合わせリストを取得
            // 候補->全能力[全能力->1能力の必要能力[1能力の必要能力->能力]]
            List<IEnumerable<IEnumerable<IAbility>>> combinations = ListCombinations(need_materials);


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
                        SynthesisInfo = (IsBasicWeapon(combweapon[0])) ? null : Synthesize(combweapon[0], false)
                    };
                    WeaponSynthesisInfo info1 = new WeaponSynthesisInfo() {
                        Weapon = combweapon[1],
                        SynthesisInfo = (IsBasicWeapon(combweapon[1])) ? null : Synthesize(combweapon[1], true)
                    };
                    WeaponSynthesisInfo info2 = (three_weapons) ? new WeaponSynthesisInfo() {
                        Weapon = combweapon[2],
                        SynthesisInfo = (IsBasicWeapon(combweapon[2])) ? null : Synthesize(combweapon[2], true)
                    }
                    : null;

                    SynthesisWeapons sw = new SynthesisWeapons() {
                        info0 = info0,
                        info1 = info1,
                        info2 = info2,
                        probabilities = probs
                    };

                    return sw;
                };

                // 3本，(目的能力数)の合成で行う場合
                foreach (var co in Assignments(3, num_slot, elements)) {
                    if (objective.Equals(co[0])) { continue; } // 目標武器に目標能力がついていたら意味がない
                    if (isMaterial && (objective.Equals(co[1]) || objective.Equals(co[2]))) { continue; } // 素材を作成している場合素材にその能力がついていたら意味がない

                    var sw = synthesis(co);
                    sw_list.Add(sw);

                    /*
                    // デバッグ用表示
                    Console.WriteLine("---");
                    foreach (var ab in co) {
                        Console.WriteLine(ab.abilities.AllToString());
                    }
                    */
                }
                
                // 3本，(目的能力数)-1の合成で行う場合
                foreach (var co in Assignments(3, num_slot - 1, elements)) {
                    if (isMaterial && (objective.Equals(co[1]) || objective.Equals(co[2]))) { continue; } // 素材を作成している場合素材にその能力がついていたら意味がない

                    var sw = synthesis(co);
                    sw_list.Add(sw);
                }

                // 2本，(目的能力数)の合成で行う場合
                foreach (var co in Assignments(2, num_slot, elements)) {
                    if (objective.Equals(co[0])) { continue; } // 目標武器に目標能力がついていたら意味がない
                    if (isMaterial && objective.Equals(co[1])) { continue; } // 素材を作成している場合素材にその能力がついていたら意味がない

                    var sw = synthesis(co);
                    sw_list.Add(sw);
                }

                // 2本，(目的能力数)-1の合成で行う場合
                foreach (var co in Assignments(2, num_slot - 1, elements)) {
                    if (isMaterial && objective.Equals(co[1])) { continue; } // 素材を作成している場合素材にその能力がついていたら意味がない

                    var sw = synthesis(co);
                    sw_list.Add(sw);
                }
            }

            SynthesisWeapons[] ret = sw_list.ToArray();
            if (!winfo_cache.ContainsKey(objective)) {
                winfo_cache.Add(objective, ret);
            }

            return ret;

        }
        #endregion (Synthesize)

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
        public static bool IsBasicWeapon(Weapon weapon)
        {
            int garbage_num = weapon.Count(ab => ab is ゴミ);

            if (garbage_num >= weapon.AbilityNum - 1) { return true; }

            return false;
        }
        #endregion (IsBasicWeapon)

        //-------------------------------------------------------------------------------
        #region ListCombinations 組み合わせを列挙
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
                bool mutation_amp = (objective is IMutationAmplifiable) && weapons.Any(weapon => weapon.ContainsAbility(ミューテーションⅠ.Get()));
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
