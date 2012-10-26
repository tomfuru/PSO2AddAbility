using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO2AddAbility
{
    public static class Util
    {
        //-------------------------------------------------------------------------------
        #region +[static]AllToString
        //-------------------------------------------------------------------------------
        //
        public static string AllToString<T>(this T[] array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            for (int i = 0; i < array.Length; i++) {
                sb.Append(array[i].ToString());
                if (i < array.Length - 1) { sb.Append(", "); }
            }
            sb.Append('}');
            return sb.ToString();
        }
        #endregion (ToString)

        //-------------------------------------------------------------------------------
        #region IEnumerable.Distinct
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
    }
}
