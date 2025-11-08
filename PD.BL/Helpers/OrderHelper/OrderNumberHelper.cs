using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Helpers.OrderHelper
{
    public class OrderNumberHelper
    {
        private static readonly Random _random = new Random();
        public static string GenerateOrderNumber()
        {
            var random = _random.Next(0, 10000);
            return random.ToString("D4");
        }
    }
}
