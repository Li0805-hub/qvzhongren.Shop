using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Shared.Helper
{
    public static class FxConvert
    {

        public static string ConvertToString(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }


        public static int ConvertToInt(this object obj)
        {
            return obj == null ? 0 : Convert.ToInt32(obj.ToString());
        }
    }
}
