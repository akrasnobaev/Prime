using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimusPrime.Templates;

namespace OptimusPrime.Toolbox
{
    public class ArrayRepeater<TIn,TOut> : IRepeaterBlock<TIn[],TOut[],TIn,TOut>
    {
        TIn[] publicIn;
        TOut[] publicOut;
        int counter = 0;

        public void Start(TIn[] publicIn)
        {
            this.publicIn = publicIn;
            publicOut = new TOut[publicIn.Length];
            counter = 0;
        }

        public bool MakeIteration(TOut oldPrivateOut, out TIn privateIn)
        {
            privateIn = publicIn[counter];
            if (counter > 0)
                publicOut[counter - 1] = oldPrivateOut;
            counter++;
            return counter >= publicIn.Length;
        }

        public TOut[] Conclude()
        {
            return publicOut;
        }
    }
}
