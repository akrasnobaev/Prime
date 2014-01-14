using System;

namespace Prime
{
    public static partial class FactoryExtension
    {
        public static IChain<TIn, Tuple<T0, T1>> Union<TIn, T0, T1>(this IPrimeFactory factory, IChain<TIn, T0> chain0,
            IChain<TIn, T1> chain1)
        {
            var block0 = chain0.ToFunctionalBlock();
            var block1 = chain1.ToFunctionalBlock();
            return factory.CreateChain<TIn, Tuple<T0, T1>>(
                z => Tuple.Create(block0.Process(z), block1.Process(z)));
        }


        public static IChain<TIn, Tuple<T0, T1, T2>> Union<TIn, T0, T1, T2>(this IPrimeFactory factory,
            IChain<TIn, T0> chain0, IChain<TIn, T1> chain1, IChain<TIn, T2> chain2)
        {
            var block0 = chain0.ToFunctionalBlock();
            var block1 = chain1.ToFunctionalBlock();
            var block2 = chain2.ToFunctionalBlock();
            return factory.CreateChain<TIn, Tuple<T0, T1, T2>>(
                z => Tuple.Create(block0.Process(z), block1.Process(z), block2.Process(z)));
        }


        public static IChain<TIn, Tuple<T0, T1, T2, T3>> Union<TIn, T0, T1, T2, T3>(this IPrimeFactory factory,
            IChain<TIn, T0> chain0, IChain<TIn, T1> chain1, IChain<TIn, T2> chain2, IChain<TIn, T3> chain3)
        {
            var block0 = chain0.ToFunctionalBlock();
            var block1 = chain1.ToFunctionalBlock();
            var block2 = chain2.ToFunctionalBlock();
            var block3 = chain3.ToFunctionalBlock();
            return factory.CreateChain<TIn, Tuple<T0, T1, T2, T3>>(
                z => Tuple.Create(block0.Process(z), block1.Process(z), block2.Process(z), block3.Process(z)));
        }


        public static IChain<TIn, Tuple<T0, T1, T2, T3, T4>> Union<TIn, T0, T1, T2, T3, T4>(this IPrimeFactory factory,
            IChain<TIn, T0> chain0, IChain<TIn, T1> chain1, IChain<TIn, T2> chain2, IChain<TIn, T3> chain3,
            IChain<TIn, T4> chain4)
        {
            var block0 = chain0.ToFunctionalBlock();
            var block1 = chain1.ToFunctionalBlock();
            var block2 = chain2.ToFunctionalBlock();
            var block3 = chain3.ToFunctionalBlock();
            var block4 = chain4.ToFunctionalBlock();
            return factory.CreateChain<TIn, Tuple<T0, T1, T2, T3, T4>>(
                z =>
                    Tuple.Create(block0.Process(z), block1.Process(z), block2.Process(z), block3.Process(z),
                        block4.Process(z)));
        }
    }
}