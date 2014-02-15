using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    class PrimeParallelChains<T> : PrimeSync<T>
        where T : Data,new()
    {
        void Run(int chainNumber, int wavesCount)
        {
            for (int i = 0; i < wavesCount; i++)
                funks[chainNumber](new T());
        }

        protected override void RunWaves()
        {
            var results = new IAsyncResult[Count];
            for (int i = 0; i < Count; i++)
                results[i] = new Action<int, int>(Run).BeginInvoke(i, WaveCount, null, null);
            for (int i = 0; i < Count; i++)
                while (!results[i].IsCompleted) Thread.Sleep(0);
        }

        public override bool IsSync
        {
            get { return false; }
        }
    }
}
