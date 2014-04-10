using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prime;

namespace PrimeProfiler
{
    class PrimeOneChain<TData,TFactory> : PrimeSync<TData,TFactory>
        where TData : Data,new()
        where TFactory : IPrimeFactory,new()
    {
      
        protected override void RunWaves()
        {
            for (int i = 0; i < Count; i++)
                for (int j = 0; j < WaveCount; j++)
                    funks[i](new TData());
        }

        public override bool IsSync
        {
            get { return true; }
        }
    }
}
