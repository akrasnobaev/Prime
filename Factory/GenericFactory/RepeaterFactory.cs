using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public  static  partial class IFactoryExtensions
    {
        public static ICallChain<TPublicIn, TPublicOut> CreateRepeater
          <TPublicIn, TPrivateIn, TPublicOut, TPrivateOut, TDataCollection>(
          IRepeaterBlock<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut> repeaterBlock,
          ISourceCollector<TDataCollection> sourceCollector, IChain<TPrivateOut, TPrivateIn> privateChaine)
          where TDataCollection : ISourceDataCollection
        {
            return new CallChain<TPublicIn, TPublicOut>(input =>
            {
                var functionalBlock = privateChaine.ToFunctionalBlock();
                TPrivateIn privateIn = repeaterBlock.Start(input);
                TPrivateOut privateOut;

                while (repeaterBlock.MakeIteration(privateIn, sourceCollector.Get(), out privateOut))
                    privateIn = functionalBlock.Process(privateOut);

                return repeaterBlock.Conclude();
            });
        }
    }
}
