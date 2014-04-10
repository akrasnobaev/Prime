using Prime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{

    class LinqParallelChains<TData> : PrimeParallelChains<TData>
    where TData : Data, new()
    {
        public LinqParallelChains()
        {
            factory = new FuncLibertyFactory();
        }
    }

    class PrimeParallelChains<T> : PrimeSync<T>
        where T : Data,new()
    {
        public PrimeParallelChains()
        {
            factory=new LibertyFactory(false);
        }

        void Run(int chainNumber, int wavesCount)
        {
            for (int i = 0; i < wavesCount; i++)
                funks[chainNumber](new T());
        }

        protected override void RunWaves()
        {
            var results = new IAsyncResult[Width];
            for (int i = 0; i < Width; i++)
                results[i] = new Action<int, int>(Run).BeginInvoke(i, WaveCount, null, null);
            for (int i = 0; i < Width; i++)
                while (!results[i].IsCompleted) Thread.Sleep(0);
        }

        public override bool IsSync
        {
            get { return false; }
        }
    }
}
