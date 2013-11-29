using OptimusPrime.Factory;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public static partial class FactoryExtensions
    {
        public static IChain<TRepeaterBigIn, TRepeaterBigOut> CreateRepeater
          <TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut, TDataCollection>(this IFactory factory,
          IRepeaterBlock<TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut, TDataCollection> repeaterBlock,
          ISourceCollector<TDataCollection> sourceCollector, IChain<TChainSmallIn, TChainSmallOut> privateChaine)
        {
            return factory.CreateChain(new FunctionalBlock<TRepeaterBigIn, TRepeaterBigOut>(
            input =>
            {
                var functionalBlock = privateChaine.ToFunctionalBlock();
                repeaterBlock.Start(input);
                TChainSmallIn smallIn;
                TChainSmallOut smallOut = default(TChainSmallOut);


                while (repeaterBlock.MakeIteration(sourceCollector.Get(), smallOut, out smallIn))
                    smallOut = functionalBlock.Process(smallIn);

                return repeaterBlock.Conclude();
            }));
        }


        public static IChain<TRepeaterBigIn, TRepeaterBigOut> CreateRepeater
           <TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut>(this IFactory factory,
                 IRepeaterBlock<TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut> repeaterBlock,
             IChain<TChainSmallIn, TChainSmallOut> privateChaine)
        {
            return factory.CreateChain(new FunctionalBlock<TRepeaterBigIn, TRepeaterBigOut>(
             input =>
             {
                 var functionalBlock = privateChaine.ToFunctionalBlock();
                 repeaterBlock.Start(input);
                 TChainSmallIn smallIn;
                 TChainSmallOut smallOut = default(TChainSmallOut);


                 while (repeaterBlock.MakeIteration(smallOut, out smallIn))
                     smallOut = functionalBlock.Process(smallIn);

                 return repeaterBlock.Conclude();
             }));
        }
    }
}