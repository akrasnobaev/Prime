using OptimusPrime.Templates;
using System;
using OptimusPrime.OprimusPrimeCore.Helpers;

namespace OptimusPrime.Factory
{
    public partial class OptimusPrimeFactory : IFactory
    {
        public IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> functionalBlock)
        {
            var inputName = ServiceNameHelper.GetInName();
            var outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusPrimeFunctionalService<TIn, TOut>(functionalBlock, inputName, outputName);

            Services.Add(service);

            return new OptimusPrimeChain<TIn, TOut>(this, service.Input, service.Output);
        }

        public IChain<TIn, T3> AddFunctionalBlockToChain<TIn, TOut, T3>(IOptimusPrimeChane<TIn, TOut> optimusPrimeChane,
                                                                             IFunctionalBlock<TOut, T3> functionalBlock)
        {
            var inputName = optimusPrimeChane.Output.Name;
            var outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusPrimeFunctionalService<TOut, T3>(functionalBlock.Process, inputName, outputName);

            Services.Add(service);

            return new OptimusPrimeChain<TIn, T3>(this, optimusPrimeChane.Input, service.Output);
        }

       

        public IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first, IChain<TMiddle, TOut> second)
        {
            var firstChain = first as IOptimusPrimeChane<TIn, TMiddle>;
            var secondChain = second as IOptimusPrimeChane<TMiddle, TOut>;
            secondChain.Input.ChangeName(firstChain.Output.Name);
            return new OptimusPrimeChain<TIn, TOut>(this, firstChain.Input, secondChain.Output);
        }
    }
}