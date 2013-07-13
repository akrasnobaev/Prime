using OptimusPrime.Templates;
using System;

namespace OptimusPrime.Factory
{
    public partial class OptimusPrimeFactory
    {
        public IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> functionalBlock)
        {
            var inputName = string.Format("{0}_in", functionalBlock.GetType().Name);
            var outputName = string.Format("{0}_out", functionalBlock.GetType().Name);
            var service = new OptimusPrimeFunctionalService<TIn, TOut>(functionalBlock, inputName, outputName);

            Services.Add(service);

            return new OptimusPrimeChain<TIn, TOut>(service.Input, service.Output);
        }

        public IChain<TIn, T3> AddFunctionalBlockToChain<TIn, TOut, T3>(IOptimusPrimeChane<TIn, TOut> optimusPrimeChane,
                                                                             IFunctionalBlock<TOut, T3> functionalBlock)
        {
            var inputName = optimusPrimeChane.Output.Name;
            var outputName = string.Format("{0}_out", functionalBlock.GetType().Name);
            var service = new OptimusPrimeFunctionalService<TOut, T3>(functionalBlock.Process, inputName, outputName);

            Services.Add(service);

            return new OptimusPrimeChain<TIn, T3>(optimusPrimeChane.Input, service.Output);
        }

        //todo: Добавить в экстеншены OptimusPrimeChain методы типа LinkChainToChain
        public IChain<T1, T3> LinkChainToChain<T1, T2, T3>(IOptimusPrimeChane<T1, T2> firstChain,
                                                                  IOptimusPrimeChane<T2, T3> secondChain)
        {
            secondChain.Input.ChangeName(firstChain.Output.Name);

            return new OptimusPrimeChain<T1, T3>(firstChain.Input, secondChain.Output);
        }
    }
}