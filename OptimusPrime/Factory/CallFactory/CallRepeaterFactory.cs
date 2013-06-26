using OptimusPrime.Templates.FunctionalItems;
using OptimusPrime.Templates.FunctionalItems.SourceCollector;
using OptimusPrime.Templates.TopologyItems.Chain.Call;

namespace OptimusPrime.Factory.CallFactory
{
    public partial class CallFactory
    {
        public ICallChain<TPublicIn, TPublicOut> CreateRepeater
            <TPublicIn, TPrivateIn, TPublicOut, TPrivateOut, TDataCollection>(
            IRepeaterBlock<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut> repeaterBlock,
            ISourceCollector<TDataCollection> sourceCollector, ICallChain<TPrivateOut, TPrivateIn> privateChaine)
            where TDataCollection : ISourceDataCollection
        {
            return new CallChain<TPublicIn, TPublicOut>(input =>
                {
                    TPrivateIn privateIn = repeaterBlock.Start(input);
                    TPrivateOut privateOut;

                    while (repeaterBlock.MakeIteration(privateIn, sourceCollector.Get(), out privateOut))
                        privateIn = privateChaine.Action(privateOut);

                    return repeaterBlock.Conclude(privateOut);
                });
        }
    }
}