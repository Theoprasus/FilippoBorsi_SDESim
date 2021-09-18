using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLN
{
    class UserInput
    {
        public double? getDouble(string input, double left_limit, double rigth_limit)
        {
            double? epsilon =null;
            if (input != null && !string.IsNullOrEmpty(input))
            {
                string e_string = input.Trim();
                e_string = e_string.Replace(",", ".");
                double e_temp = 0;

                if (double.TryParse(e_string, System.Globalization.NumberStyles.Any, new CultureInfo("en-US"), out e_temp))
                {
                    if (e_temp <= rigth_limit && e_temp >= left_limit)
                        epsilon = e_temp;
                }


            }
            return epsilon;




        }
        public int? getInt(string input, double left_limit, double rigth_limit)
        {
            int? val = null;
            if (input != null && !string.IsNullOrEmpty(input))
            {
                string e_string = input.Trim();
                e_string = e_string.Replace(",", ".");
                int e_temp = 0;

                if (int.TryParse(e_string, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out e_temp))
                {
                    if (e_temp <= rigth_limit && e_temp >= left_limit)
                        val = e_temp;
                }


            }
            return val;




        }
    }
}
