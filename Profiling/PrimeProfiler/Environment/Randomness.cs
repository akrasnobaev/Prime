using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    public class Rnd
    {
        static Random random = new Random(1);

        public static double Double() { return random.NextDouble(); }
    }
}
