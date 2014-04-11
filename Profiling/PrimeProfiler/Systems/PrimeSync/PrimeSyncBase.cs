using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prime;

namespace PrimeProfiler
{
    abstract class PrimeSync<TData> : TestSystem
        where TData : Data, new ()
    {

        protected Func<TData, TData>[] funks;
        protected IPrimeFactory factory;

        protected override void Initialize()
        {
            funks = new Func<TData, TData>[Width];
            for (int i = 0; i < Width; i++)
            {
                var chain = factory.CreateChain<TData, TData>(z => { z.Marker=Computations.Compute(z.Marker); return z; });
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
            get { return typeof(TData); }
        }
    }
}
