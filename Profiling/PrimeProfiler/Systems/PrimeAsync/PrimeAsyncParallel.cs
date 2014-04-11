using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prime;

namespace PrimeProfiler
{
    class PrimeAsyncParallel<T> : PrimeAsync<T>
        where T : Data,new()
    {

        protected void RunChain(int chainNum)
        {
            for (int  i=0;i<WaveCount;i++)
            {
                Inputs[chainNum].Publish(new T());
                Outputs[chainNum].Get();
            }
        }

        protected override void RunWaves()
        {
            var results = new IAsyncResult[Width];
            for (int i = 0; i < Width; i++)
                results[i] = new Action<int>(RunChain).BeginInvoke(i, null, null);
            for (int i = 0; i < Width; i++)
                while (!results[i].IsCompleted) Thread.Sleep(0);
        }

        
    }
}
