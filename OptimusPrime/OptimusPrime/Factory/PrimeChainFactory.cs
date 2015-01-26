using System;
using Prime.Optimus;

namespace Prime
{
    public partial class PrimeFactory
    {
        public override IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> functionalBlock,
            string pseudoName = null)
        {
            var inputName = ServiceNameHelper.GetInName();
            var outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusFunctionalService<TIn, TOut>(functionalBlock, inputName, outputName, Stopwatch, IsLogging);

            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            Services.Add(service);

            return new OptimusChain<TIn, TOut>(this, service.Input, service.Output);
        }

        public override IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first,
            IChain<TMiddle, TOut> second)
        {
            // Помечаем цепочки как использованные.
            first.MarkUsed();
            second.MarkUsed();
            
            var firstChain = first as IOptimusChain<TIn, TMiddle>;
            var secondChain = second as IOptimusChain<TMiddle, TOut>;
            secondChain.Input.ChangeName(firstChain.Output.Name);
            return new OptimusChain<TIn, TOut>(this, firstChain.Input, secondChain.Output);
        }
    }
}