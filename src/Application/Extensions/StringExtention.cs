using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Application.Extensions
{
    public static class StringExtention
    {

        public static decimal ToDecimal(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return 0;

            str =  str.Replace(",",".");

            return decimal.Parse(str);
        }


        public static long ToLong(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return 0;

            return long.Parse(str);
        }
    }
}
