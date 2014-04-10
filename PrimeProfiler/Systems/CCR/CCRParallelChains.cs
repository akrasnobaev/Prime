using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    /// <summary>
    /// Signal passes syncronously through the system. The real example for that is a situation when the new signal actually 
    /// appears after the last processing is done (i.e. status appears after command is fed to effectors)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CCRParallelChains<T> : CCRBase<T>
        where T : Data,new()
    {

        protected void RunOneChain(int chainNumber, int wavesCount)
        {
            for (int i = 0; i < wavesCount; i++)
            {
                InputPorts[chainNumber].Post(new T());
                while (flags[chainNumber] < i + 1) Thread.Sleep(0);
            }
        }


        protected override void RunWaves()
        {
            var results = new IAsyncResult[Width];
            for (int i = 0; i < Width; i++)
                results[i] = new Action<int, int>(RunOneChain).BeginInvoke(i, WaveCount, null, null);

            for (int i = 0; i < Width; i++)
                while (!results[i].IsCompleted) Thread.Sleep(0);
        }

        public override bool IsSync
        {
            get { return false; }
        }
       
    }
}
