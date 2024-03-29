﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSO2AddAbility
{
    public static class Util
    {
        //-------------------------------------------------------------------------------
        #region +[static]AllToString
        //-------------------------------------------------------------------------------
        //
        public static string AllToString<T>(this T[] array, char first = '{', char last = '}')
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(first);
            for (int i = 0; i < array.Length; i++) {
                sb.Append(array[i].ToString());
                if (i < array.Length - 1) { sb.Append(", "); }
            }
            sb.Append(last);
            return sb.ToString();
        }
        #endregion (ToString)

        //-------------------------------------------------------------------------------
        #region +[extension:IEnumerable]IEnumerable.Distinct
        //-------------------------------------------------------------------------------
        /*private class MyEqualityComparer<T> : IEqualityComparer<T>
        {
            private Func<T, T, bool> _eqComparer;
            public MyEqualityComparer(Func<T, T, bool> eqComparer) { _eqComparer = eqComparer; }

            bool IEqualityComparer<T>.Equals(T x, T y)
            {
                return _eqComparer(x, y);
            }

            int IEqualityComparer<T>.GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }*/
        //
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable, Func<T, T, bool> equalitycomparison)
        {
            List<T> l = new List<T>();
            foreach (var item in enumerable) {
                if (l.Any(item2 => equalitycomparison(item, item2))) { continue; }
                l.Add(item);
            }
            return l;

            // うまく動かない(全くDistinctしない)
            //MyEqualityComparer<T> comparer = new MyEqualityComparer<T>(equalitycomparison);
            //return enumerable.Distinct(comparer);
        }
        #endregion (IEnumerable.Distinct)

        //-------------------------------------------------------------------------------
        #region +[static]IsSoulAmplifiable
        //-------------------------------------------------------------------------------
        //
        public static bool IsSoulAmplifiable<T>(IAbility ability, T soul) where T : Soul
        {
            return (ability is ISoulAmplifiable<T>);
        }
        #endregion (Name)

        //-------------------------------------------------------------------------------
        #region +[static]ProbabilityToString 確率を文字列に
        //-------------------------------------------------------------------------------
        //
        public static string ProbabilityToString(float f)
        {
            return ((int)(f * 100)).ToString() + '%';
        }
        #endregion (ProbabilityToString)

        //-------------------------------------------------------------------------------
        #region +[static]CostToString コストを文字列に
        //-------------------------------------------------------------------------------
        //
        public static string CostToString(double cost, string format = null)
        {
            string str = (format != null) ? ((long)cost).ToString(format) : ((long)cost).ToString();
            return Double.IsNaN(cost) ? "コスト不明" : str + "メセタ";
        }
        #endregion (CostToString)

        //-------------------------------------------------------------------------------
        #region +[static]AllProbability 全てが成功する確率
        //-------------------------------------------------------------------------------
        //
        public static float AllProbability(float[] probabilities)
        {
            return probabilities.Aggregate(1.0f, (f1, f2) => f1 * f2);
        }
        #endregion (AllProbability)

        //-------------------------------------------------------------------------------
        #region +[extension:ScrollBar]ScrollDelta
        //-------------------------------------------------------------------------------
        //
        public static void ScrollDelta(this ScrollBar scr, int change)
        {
            int value = Math.Min(Math.Max(scr.Value + change, scr.Minimum), scr.Maximum);
            scr.Value = value;
            //scr.Value = value; // 2回しないとなぜか反応しない
        }
        #endregion (ScrollDelta)

        //-------------------------------------------------------------------------------
        #region +[static]StrToIAbility 文字列をアビリティに
        //-------------------------------------------------------------------------------
        //
        public static IAbility StrToIAbility(string str)
        {
            int level = (str.EndsWith("Ⅰ")) ? 1 :
                        (str.EndsWith("Ⅱ")) ? 2 :
                        (str.EndsWith("Ⅲ")) ? 3 :
                        (str.EndsWith("Ⅳ")) ? 4 :
                        (str.EndsWith("Ⅴ")) ? 5 : 0;

            string ability_str = (level > 0) ? str.Substring(0, str.Length - 1) : str;
            ability_str = ability_str.Replace("・", ""); 
            AbilityType type;
            if (!Enum.TryParse<AbilityType>(ability_str, out type)) { return null; }

            IAbility ab = Data.DIC_ABILITYTYPE_TO_IABILITY[type];

            if (level > 0) {
                return (ab as ILevel).GetInstanceOfLv(level);
            }
            else { return ab; }
        }
        #endregion (StrToIAbility)
    }
}
