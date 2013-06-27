using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimusPrime.Templates.TopologyItems.Chain.OptimusPrime
{
    public interface IOptimusPrimeChane<TIn, TOut> : IChain<TIn, TOut>
    {
        IOptimusPrimeIn Input { get; }
        IOptimusPrimeOut Output { get; }
    }
}