using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    public class For<T> : TestSystem
        where T : Data, new()
    {

        protected override void Initialize()
        {
        }

        protected void RunWave()
        {
            for (int j = 0; j < WaveCount; j++)
            {
                var t = new T();
                for (int i = 0; i < Length; i++)
                {
                    t.Marker = Computations.Compute(t.Marker);
                }
            }
        }

        protected override void RunWaves()
        {
            var results = new IAsyncResult[Width];
            for (int i = 0; i < Width; i++)
                results[i] = new Action(RunWave).BeginInvoke(null, null);
            for (int i = 0; i < Width; i++)
                while (!results[i].IsCompleted) Thread.Sleep(0);
        }

        protected override void Finish()
        {
           
        }

        public override bool IsSync
        {
            get { return true; }
        }

        public override Type GetDataType
        {
            get { return typeof(T); }
        }
    }
}
