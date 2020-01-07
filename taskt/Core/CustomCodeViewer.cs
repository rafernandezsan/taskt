using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public static class CustomCode
    {
        public static DateTime ReformatDate(string LongDate)
        {

            char separator = ' ';
            String[] str = LongDate.Split(separator);

            int Month = GetMonthIndex(str[2]) + 1; // Se suma uno porque Enero es 0...

            string fecha = str[0] + '/' + Month + '/' + str[4];
            DateTime dateTime = Convert.ToDateTime(fecha);
            return dateTime;

        }

        public static int GetMonthIndex(this string month)
        {
            return Array.FindIndex(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames,
                             t => t.Equals(month, StringComparison.CurrentCultureIgnoreCase));
        }

        static void CustomMain(string[] args) //Main of the custom code, it is required to change to Main
        {
            if (args.Length > 0)
            {
                if (args[0] == "CALC_DATE")
                {
                    DateTime fecha = ReformatDate(args[1]);
                    Console.WriteLine("{0}", fecha);
                }
                else
                {
                    Console.WriteLine("Argumento no valido.");
                }
            }
            else
            {
                Console.WriteLine("Argumento no valido.");
            }

        }

    }
}
