namespace Prime
{
    public class ArrayRepeater<TIn, TOut> : IRepeaterBlock<TIn[], TOut[], TIn, TOut>
    {
        private TIn[] publicIn;
        private TOut[] publicOut;
        private int counter = 0;

        public void Start(TIn[] publicIn)
        {
            this.publicIn = publicIn;
            publicOut = new TOut[publicIn.Length];
            counter = 0;
        }

        public bool MakeIteration(TOut oldPrivateOut, out TIn privateIn)
        {
            if (counter >= publicIn.Length)
            {
                privateIn = default(TIn);
                return false;
            }

            privateIn = publicIn[counter];
            if (counter > 0)
                publicOut[counter - 1] = oldPrivateOut;
            counter++;
            return true;
        }

        public TOut[] Conclude()
        {
            return publicOut;
        }
    }
}