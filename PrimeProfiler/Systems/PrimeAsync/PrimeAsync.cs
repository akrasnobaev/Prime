using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prime;

namespace PrimeProfiler
{
    class PrimeAsync<T> : TestSystem
        where T : Data,new()
    {
        protected SourceBlock<T>[] Inputs;
        protected ISourceReader<T>[] Outputs;
        IPrimeFactory factory;

        protected override void Initialize()
        {
            factory=new LibertyFactory();
            Inputs=new SourceBlock<T>[Count];
            Outputs = new ISourceReader<T>[Count];
            for (int i = 0; i < Count; i++)
            {
                Inputs[i] = new SourceBlock<T>();
                var source = factory.CreateSource(Inputs[i]);
                for (int j = 0; j < Length; j++)
                    source = source.Link(z => { z.Marker=Computations.Compute(z.Marker); return z; });
                Outputs[i] = source.CreateReader();
            }
            factory.Start();
        }

        protected override void RunWaves()
        {
            for (int i = 0; i < Count; i++)
                for (int j = 0; j < WaveCount; j++)
                    Inputs[i].Publish(new T());

            for (int i = 0; i < Count; i++)
            {
                int n = 0;
                while (true)
                {
                    n += Outputs[i].GetCollection().Length;
                    if (n >= WaveCount) break;
                }
            }
        }

        protected override void Finish()
        {
            factory.Stop();
        }

        public override bool IsSync
        {
            get { return false; }
        }

        public override Type GetDataType
        {
            get { return typeof(T); }
        }
    }
}
