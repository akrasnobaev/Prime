using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prime;

namespace PrimeProfiler
{
    abstract class PrimeSync<T> : TestSystem
        where T : Data, new ()
    {

        protected Func<T, T>[] funks;
        IPrimeFactory factory;

        protected override void Initialize()
        {
            funks = new Func<T, T>[Count];
            factory = new LibertyFactory();
            for (int i = 0; i < Count; i++)
            {
                var chain = factory.CreateChain<T, T>(z => { z.Marker=Computations.Compute(z.Marker); return z; });
                for (int j = 0; j < Length - 1; j++)
                    chain = chain.Link(z => { z.Marker = Computations.Compute(z.Marker); return z; });
                funks[i] = chain.ToFunctionalBlock().Process;
            }
            factory.Start();
        }


        protected override void Finish()
        {
            factory.Stop();
        }

        public override Type GetDataType
        {
            get { return typeof(T); }
        }
    }
}
