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
                if (i < array.Length - 1) { sb.Append(" ,"); }
            }
            sb.Append('}');
            return sb.ToString(); 
        }
        #endregion (ToString)
    }
}
