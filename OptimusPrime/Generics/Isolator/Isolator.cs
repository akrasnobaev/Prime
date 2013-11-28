using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime
{
    public class Isolator<TIn,TOut>
    {
        IChain<TIn, TOut> isolatedChain;
        IFunctionalBlock<TIn, TOut> funcBlock;
        public Isolator(IChain<TIn, TOut> isolated)
        {
            this.isolatedChain = isolated;
            this.funcBlock = isolatedChain.ToFunctionalBlock();
        }
        object lockObject = new object();
        public IChain<TIn, TOut> CreateIsolated()
        {
            return isolatedChain.Factory.CreateChain(
                new Func<TIn, TOut>(
                    z =>
                    {
                        TOut result = default(TOut);
                        lock (lockObject)
                        {
                            result = funcBlock.Process(z);
                        }
                        return result;
                    }));
        }
    }
}
