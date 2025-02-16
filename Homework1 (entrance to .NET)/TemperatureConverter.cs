using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    internal static class TemperatureConverter
    {
        public static decimal GetFromCelsiumToFarenheit(decimal temperature)
            => (temperature * 9 / 5) + 32;

        public static decimal GetFromFarenheitToCelsium(decimal temperature)
            => (temperature - 32) * 5 / 9;
    }
}
