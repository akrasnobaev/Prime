using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    /// <summary>
    /// At every moment, only one service run. Quite inefficient, of course.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CCROneChain<T> : CCRParallelChains<T>
        where T : Data,new()
    {
        protected override void RunWaves()
        {
            for (int i = 0; i < Width; i++)
                RunOneChain(i, WaveCount);
        }

        public override bool IsSync
        {
            get { return true; }
        }

    }
}
