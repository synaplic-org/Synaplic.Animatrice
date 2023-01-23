using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Shared.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToInputString(this decimal value, string format = "#.##")
        {
            return value.ToString(format).Replace(",", ".");
        }

        public static string ToValueString(this decimal value, string format = "N2")
        {
            if (value == 0) return "";

            return value.ToString(format).Replace(",", ".");
        }

        public static string ToCoefString(this decimal value, string format = "#.###")
        {
            if (value == 0) return "";

            return value.ToString(format).Replace(",", ".") + "%";
        }

        public static string ToCSString(this decimal value)
        {
            return value.ToString().Replace(",", ".");
        }



    }
}
