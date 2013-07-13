using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public  static  partial class IFactoryExtensions
    {
        public static ICallChain<TRepeaterBigIn, TRepeaterBigOut> CreateRepeater
          <TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut, TDataCollection>(this IFactory factory,
          IRepeaterBlock<TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut, TDataCollection> repeaterBlock,
          ISourceCollector<TDataCollection> sourceCollector, IChain<TChainSmallIn, TChainSmallOut> privateChaine)
          where TDataCollection : ISourceDataCollection
        {
            return new CallChain<TRepeaterBigIn, TRepeaterBigOut>(input =>
            {
                var functionalBlock = privateChaine.ToFunctionalBlock();
                repeaterBlock.Start(input);
                TChainSmallIn smallIn;
                TChainSmallOut smallOut = default(TChainSmallOut);

                
                while (repeaterBlock.MakeIteration(sourceCollector.Get(), smallOut, out smallIn))
                    smallOut = functionalBlock.Process(smallIn);

                return repeaterBlock.Conclude();
            });
        }
    }
}
