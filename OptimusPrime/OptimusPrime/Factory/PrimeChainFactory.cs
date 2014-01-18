using System;
using Prime.Optimus;

namespace Prime
{
    public partial class PrimeFactory
    {
        public IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> functionalBlock, string pseudoName = null)
        {
            var inputName = ServiceNameHelper.GetInName();
            var outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusFunctionalService<TIn, TOut>(functionalBlock, inputName, outputName, Stopwatch);

            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            Services.Add(service);

            return new OptimusChain<TIn, TOut>(this, service.Input, service.Output);
        }

        public IChain<TIn, T3> AddFunctionalBlockToChain<TIn, TOut, T3>(IOptimusChane<TIn, TOut> optimusChane,
            IFunctionalBlock<TOut, T3> functionalBlock, string pseudoName = null)
        {
            var inputName = optimusChane.Output.Name;
            var outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusFunctionalService<TOut, T3>(functionalBlock.Process, inputName, outputName, Stopwatch);

            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            Services.Add(service);

            return new OptimusChain<TIn, T3>(this, optimusChane.Input, service.Output);
        }


        public IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first,
            IChain<TMiddle, TOut> second)
        {
            var firstChain = first as IOptimusChane<TIn, TMiddle>;
            var secondChain = second as IOptimusChane<TMiddle, TOut>;
            secondChain.Input.ChangeName(firstChain.Output.Name);
            return new OptimusChain<TIn, TOut>(this, firstChain.Input, secondChain.Output);
        }
    }
}