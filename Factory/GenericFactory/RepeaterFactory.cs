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
        public static ICallChain<TPublicIn, TPublicOut> CreateRepeater
          <TPublicIn, TPrivateIn, TPublicOut, TPrivateOut, TDataCollection>(
          IRepeaterBlock<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut> repeaterBlock,
          ISourceCollector<TDataCollection> sourceCollector, IChain<TPrivateOut, TPrivateIn> privateChaine)
          where TDataCollection : ISourceDataCollection
        {
            return new CallChain<TPublicIn, TPublicOut>(input =>
            {
                var callPrivateChain = privateChaine.ToCallChain();
                TPrivateIn privateIn = repeaterBlock.Start(input);
                TPrivateOut privateOut;

                while (repeaterBlock.MakeIteration(privateIn, sourceCollector.Get(), out privateOut))
                    privateIn = callPrivateChain.Action(privateOut);

                return repeaterBlock.Conclude();
            });
        }
    }
}
