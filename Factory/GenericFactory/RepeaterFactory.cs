using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var callPrivateChain = privateChaine.ToCallChain();
                repeaterBlock.Start(input);
                TChainSmallIn smallIn;
                TChainSmallOut smallOut = default(TChainSmallOut);

                
                while (repeaterBlock.MakeIteration(sourceCollector.Get(), smallOut, out smallIn))
                    smallOut = callPrivateChain.Action(smallIn);

                return repeaterBlock.Conclude();
            });
        }
    }
}
