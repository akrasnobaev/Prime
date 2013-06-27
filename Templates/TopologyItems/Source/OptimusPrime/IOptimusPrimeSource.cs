using OptimusPrime.OprimusPrimeCore.Service;

namespace OptimusPrime.Templates.TopologyItems.Source.OptimusPrime
{
    public interface IOptimusPrimeSource<TPublic> : ISource<TPublic>
    {
        IOptimusPrimeOut Output { get; }
    }
}