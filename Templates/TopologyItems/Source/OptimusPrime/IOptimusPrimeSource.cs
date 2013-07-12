using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    public interface IOptimusPrimeSource<TPublic> : ISource<TPublic>
    {
        IOptimusPrimeOut Output { get; }
    }
}