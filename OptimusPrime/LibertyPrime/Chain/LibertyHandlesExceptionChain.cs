using System;
using OptimusPrime;

namespace Prime.Liberty
{
    public class LibertyHandlesExceptionChain<TIn, TInnerOut> : LibertyChain<TIn, PrimeData<TInnerOut>>,
        IHandlesExceptionChain<TIn, TInnerOut>
    {
        private Func<Exception, TInnerOut> exceptionHandler;

        public LibertyHandlesExceptionChain(ILibertyChain<TIn, PrimeData<TInnerOut>> chain)
            : base(chain.Factory, chain.Action, chain.OutputName)
        {
        }

        public void HandleExceptions(Func<Exception, TInnerOut> handler)
        {
            exceptionHandler = handler;
        }

        public new IFunctionalBlock<TIn, TInnerOut> ToFunctionalBlock()
        {
            MarkUsed();
            var smartClone = new SmartClone<TIn>();
            return new FunctionalBlock<TIn, TInnerOut>(data =>
            {
                var result = Action(smartClone.Clone(data));
                if (result.Exception != null && exceptionHandler == null)
                    throw result.Exception;
                return result.Exception != null
                    ? exceptionHandler(result.Exception)
                    : result.Data;
            });
        }
    }
}