using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{
    public static class Synthesis
    {
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
        #region +Synthesize
        //-------------------------------------------------------------------------------
        //
        public static void Synthesize(Weapon objective)
        {
            int num_slot = objective.abilities.Length;
            // 必要な素材能力の候補リストを取得
            var need_materials = objective.abilities.Select(ab => Data.GetMaterialAbilities(ab)).ToArray();

            // 1能力に必要な能力が複数ある(パワーⅢはパワーⅡ*2でも3でも作れる等)ため，それらの候補のすべての組み合わせリストを取得
            // 候補->全能力[全能力->1能力の必要能力[1能力の必要能力->能力]]
            List<IEnumerable<IEnumerable<IAbility>>> combinations = ListCombinations(need_materials);

            // 各素材の組み合わせについて合成方法を調べる
            foreach (var combination in combinations) {
                List<AbilityInfo> elements = new List<AbilityInfo>(); // 必要特殊能力リスト
                foreach (var elem in combination) {
                    IAbility[] elemarray = elem.ToArray();
                    IAbility[] distinct = elem.Distinct().ToArray();
                    foreach (var element in distinct) {
                        int num = elemarray.Count(el => el.GetType().Equals(element.GetType()));
                        int index = elements.FindIndex(ai => ai.Ability.GetType().Equals(element.GetType()));

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


                // 3本，(目的能力数)-1の合成で行う場合

                // 3本，(目的能力数)の合成で行う場合

                // 2本，(目的能力数)-1の合成で行う場合

                // 2本，(目的能力数)の合成で行う場合

            }

        }
        #endregion (Synthesize)

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
        #region -Assignments 特殊能力の振り分けリスト
        //-------------------------------------------------------------------------------
        //
        private static IEnumerable<int> Assignments(int weapon_num, int slot_num, List<AbilityInfo> elements)
        {
            if (weapon_num != 2 && weapon_num != 3) { throw new NotSupportedException("武器の個数は2個か3個のみ"); }

            if (weapon_num * slot_num < elements.Sum(ai => ai.Num)) { yield break; }  // 必要特殊能力数が武器数*スロット数より多いなら不可能
            if (elements.Max(ai => ai.Num) > weapon_num) { yield break; }             // 同じ特殊能力が武器の個数より多く必要なら不可能

            int[][][] combinations = COMBINATION[weapon_num];

            // { {{0},{1}}, {{0,1}} } => { {{0}, {0,1}}, {{1}, {0,1}} }
            //var list = ListCombinations<IEnumerable<int>>(elements.Select(ai => combinations[ai.Num]).ToArray());

            //foreach (var combination in list) {
            //    combination
            //}

            throw new NotImplementedException();
        }
        #endregion (Assignments)

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
