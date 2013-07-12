using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public interface IOptimusPrimeChane<TIn, TOut> : IChain<TIn, TOut>
    {
        IOptimusPrimeIn Input { get; }
        IOptimusPrimeOut Output { get; }
    }
}