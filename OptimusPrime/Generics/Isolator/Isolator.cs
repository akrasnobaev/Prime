using OptimusPrime.Templates;
using System;

namespace OptimusPrime.Factory
{
    public class Isolator<TIn, TOut>
    {
        private IChain<TIn, TOut> isolatedChain;
        private IFunctionalBlock<TIn, TOut> funcBlock;
        private object lockObject = new object();

        public Isolator(IChain<TIn, TOut> isolated)
        {
            isolatedChain = isolated;
            funcBlock = isolatedChain.ToFunctionalBlock();
        }

        public IChain<TIn, TOut> CreateIsolated()
        {
            return isolatedChain.Factory.CreateChain(
                new Func<TIn, TOut>(
                    z =>
                    {
                        TOut result;
                        lock (lockObject)
                        {
                            result = funcBlock.Process(z);
                        }
                        return result;
                    }));
        }
    }
}
