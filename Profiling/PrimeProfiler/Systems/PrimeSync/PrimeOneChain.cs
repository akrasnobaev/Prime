using Prime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    class LinqOneChain<TData> : PrimeOneChain<TData>
        where TData : Data, new()
    {
        public LinqOneChain()
        {
            factory = new FuncLibertyFactory();
        }
    }

    class PrimeOneChain<TData> : PrimeSync<TData>
        where TData : Data,new()
    {
        public PrimeOneChain()
        {
            factory = new LibertyFactory(false);
        }
      
        protected override void RunWaves()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < WaveCount; j++)
                    funks[i](new TData());
        }

        public override bool IsSync
        {
            get { return true; }
        }
    }
}
