using OptimusPrime.Templates.FunctionalItems;
using OptimusPrime.Templates.FunctionalItems.SourceCollector;
using OptimusPrime.Templates.TopologyItems.Chain.OptimusPrime;
using OptimusPrime.Templates.TopologyItems.Repeater.OptimusPrime;

namespace OptimusPrime.Factory.OptimusPrimeFactory
{
    public partial class OptimusPrimeFactory
    {
        public IOptimusPrimeChane<TPublicIn, TPublicOut> CreateRepeater
            <TPublicIn, TPrivateIn, TPublicOut, TPrivateOut, TDataCollection>(
            IRepeaterBlock<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut> repeaterBlock,
            ISourceCollector<TDataCollection> sourceCollector, IOptimusPrimeChane<TPrivateOut, TPrivateIn> privateChaine)
            where TDataCollection : ISourceDataCollection
        {
            string inputName = string.Format("{0}_in", repeaterBlock.GetType().Name);
            string outputName = string.Format("{0}_out", repeaterBlock.GetType().Name);
            var service = new OptimusPrimeRepeaterService
                <TPublicIn, TPrivateIn, TPublicOut, TPrivateOut, TDataCollection>(
                repeaterBlock, sourceCollector, inputName, privateChaine.Output.Name, outputName,
                privateChaine.Input.Name);

            Services.Add(service);

            return new OptimusPrimeChain<TPublicIn, TPublicOut>(service.Input, service.Output);
        }
    }
}