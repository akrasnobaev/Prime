using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    class PrimeOneChain<T> : PrimeSync<T>
        where T : Data,new()
    {
      
        protected override void RunWaves()
        {
            for (int i = 0; i < Count; i++)
                for (int j = 0; j < WaveCount; j++)
                    funks[i](new T());
        }

        public override bool IsSync
        {
            get { return true; }
        }
    }
}
