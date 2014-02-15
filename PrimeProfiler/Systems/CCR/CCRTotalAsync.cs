using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    /// <summary>
    /// A normal work for CCR: all the tasks are given at the initial time, and then services proceed as they wishes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CCRTotalAsync<T> : CCRBase<T>
        where T : Data,new()
    {
        protected override void RunWaves()
        {
            for (int i = 0; i < WaveCount; i++)
                for (int j = 0; j < Count; j++)
                    InputPorts[j].Post(new T());


            for (int i = 0; i < Count; i++)
                while (flags[i] < WaveCount-3) Thread.Sleep(0);

        }

        public override bool IsSync
        {
            get { return false; }
        }
    }
}
