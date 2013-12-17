using OptimusPrime.Factory;
using OptimusPrime.Generics;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public static partial class FactoryExtensions
    {
       


        public static IChain<TRepeaterBigIn, TRepeaterBigOut> CreateRepeater
  <TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut, TDataCollection>(this IFactory factory,
  IRepeaterBlock<TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut, TDataCollection> repeaterBlock,
  IChain<CollectorRequest,TDataCollection> sourceCollector, IChain<TChainSmallIn, TChainSmallOut> privateChaine)
        {
            return factory.CreateChain(new FunctionalBlock<TRepeaterBigIn, TRepeaterBigOut>(
            input =>
            {
                var collectorBlock = sourceCollector.ToFunctionalBlock();
                var functionalBlock = privateChaine.ToFunctionalBlock();
                repeaterBlock.Start(input);
                TChainSmallIn smallIn;
                TChainSmallOut smallOut = default(TChainSmallOut);


                while (repeaterBlock.MakeIteration(collectorBlock.Process(CollectorRequest.Get), smallOut, out smallIn))
                    smallOut = functionalBlock.Process(smallIn);

                collectorBlock.Process(CollectorRequest.Pushbask);
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