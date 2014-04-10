using System;
using System.Linq.Expressions;
using OptimusPrime.Common.Exception;
using Prime;

namespace OptimusPrime.FuncLibertyPrime
{
    public class FuncLibertyChain<TIn, TOut> : IFuncLibertyChain<TIn, TOut>
    {
        private bool isUsed;

        public FuncLibertyChain(IPrimeFactory factory, Expression<Func<TIn, TOut>> expression, string outputName)
        {
            Factory = factory;
            OutputName = outputName;
            Expression = expression;
            isUsed = false;
        }

        public IFunctionalBlock<TIn, TOut> ToFunctionalBlock()
        {
            MarkUsed();

            var smartClone = new SmartClone<TIn>();
            var action = Expression.Compile();

            return new FunctionalBlock<TIn, TOut>(data => action(smartClone.Clone(data)));
        }

        public IPrimeFactory Factory { get; private set; }
        public string InputName { get; private set; }
        public string OutputName { get; private set; }

        public void MarkUsed()
        {
            if (isUsed)
                throw new ChainAlreadyUsedException();
            isUsed = true;
        }

        public Expression<Func<TIn, TOut>> Expression { get; private set; }
    }
}