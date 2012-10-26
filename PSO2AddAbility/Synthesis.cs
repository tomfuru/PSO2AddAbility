using System;
using System.Collections.Generic;
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
            if (winfo_cache.ContainsKey(objective)) {
                return winfo_cache[objective];
            }
            else if (IsBasicWeapon(objective)) {
                return null;
            }

            int num_slot = objective.abilities.Length;
            // 必要な素材能力の候補リストを取得
            var need_materials = objective.abilities.Select(ab => Data.GetMaterialAbilities(ab)).ToArray();

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


                // 3本，(目的能力数)の合成で行う場合
                foreach (var co in Assignments(3, num_slot, elements)) {
                    if (objective.Equals(co[0])) { continue; } // 目標武器に目標能力がついていたら意味がない
                    if (isMaterial && (objective.Equals(co[1]) || objective.Equals(co[2]))) { continue; }
                   
                    WeaponSynthesisInfo info0 = new WeaponSynthesisInfo() { 
                        Weapon = co[0],
                        SynthesisInfo = (IsBasicWeapon(co[0])) ? null : Synthesize(co[0], false)
                    };
                    WeaponSynthesisInfo info1 = new WeaponSynthesisInfo() { 
                        Weapon = co[1],
                        SynthesisInfo = (IsBasicWeapon(co[1])) ? null : Synthesize(co[1], true)
                    };
                    WeaponSynthesisInfo info2 = new WeaponSynthesisInfo() { 
                        Weapon = co[2],
                        SynthesisInfo = (IsBasicWeapon(co[2])) ? null : Synthesize(co[2], true)
                    };

                    SynthesisWeapons sw = new SynthesisWeapons() {
                        info0 = info0,
                        info1 = info1,
                        info2 = info2
                    };

                    sw_list.Add(sw);

                    /*
                    // デバッグ用表示
                    Console.WriteLine("---");
                    foreach (var ab in co) {
                        Console.WriteLine(ab.abilities.AllToString());
                    }
                    */

                    // TODO:BasicWeaponか判別，そうでないなら再帰的に合成過程を調べる


                }


                /*
                // 3本，(目的能力数)-1の合成で行う場合
                foreach (var co in Assignments(3, num_slot - 1, elements)) {
                    // TODO:BasicWeaponか判別，そうでないなら再帰的に合成過程を調べる
                }

                // 2本，(目的能力数)の合成で行う場合
                foreach (var co in Assignments(2, num_slot, elements)) {
                    if (objective.Equals(co.ElementAt(0))) { continue; } // 目標武器に目標能力がついていたら意味がない
                    // TODO:BasicWeaponか判別，そうでないなら再帰的に合成過程を調べる
                }

                // 2本，(目的能力数)-1の合成で行う場合
                foreach (var co in Assignments(2, num_slot - 1, elements)) {
                    // TODO:BasicWeaponか判別，そうでないなら再帰的に合成過程を調べる
                }
                */
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
                                       new Weapon {
                                           abilities = indicies.Select(index => elements[index].Ability)
                                                               .Concat(Enumerable.Repeat(ゴミ.Get(), slot_num - indicies.Length)).ToArray()
                                       }
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
            int garbage_num = weapon.abilities.Count(ab => ab is ゴミ);

            if (garbage_num >= weapon.abilities.Length - 1) { return true; }

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
        #region +GetProbability
        //-------------------------------------------------------------------------------
        //
        public static float GetProbability(IAbility objective, Weapon weapon1, Weapon weapon2, Weapon weapon3 = null)
        {

            throw new NotImplementedException();
        }
        #endregion (GetProbability)

        //-------------------------------------------------------------------------------
        #region +GetProbabilities
        //-------------------------------------------------------------------------------
        //
        public static float[] GetProbabilities(Weapon objective, Weapon weapon1, Weapon weapon2, Weapon weapon3 = null)
        {

            throw new NotImplementedException();
        }
        #endregion (GetProbabilities)
    }
}
