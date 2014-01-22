using System;
using Prime.Optimus;

namespace Prime
{
    public partial class PrimeFactory
    {
        public override IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> functionalBlock, string pseudoName = null)
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
    }
}