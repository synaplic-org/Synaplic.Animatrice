using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Shared.Extensions
{
   public static class StringExtentions
    {
        public static string ToDateString(this string dateString, string format = "o",string culture = "fr-FR")
        {
            var dateTime = DateTime.Parse(dateString, new System.Globalization.CultureInfo(culture),  System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            return dateTime.ToString(format) ;
        }
        public static DateTimeOffset ToDateTimeOffset(this string dateString,  string culture = "fr-FR")
        {
            var dateTime = DateTime.Parse(dateString, new System.Globalization.CultureInfo(culture), System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            return dateTime;
        }
        public static DateTime ToDateTime(this string dateString, string culture = "fr-FR")
        {
            var dateTime = DateTime.Parse(dateString, new System.Globalization.CultureInfo(culture), System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            return dateTime;
        }

        public static string ToDecimalString(this string dateString, string format = "N2")
        {
            decimal x = 0;
            Decimal.TryParse(dateString, out x);
            return x.ToValueString(format);
        
           
        }
    }
}
 
