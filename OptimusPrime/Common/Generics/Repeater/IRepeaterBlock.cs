namespace Prime
{
    public interface IRepeaterBlock<TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut,
        TSourceDataCollection>
    {
        void Start(TRepeaterBigIn publicIn);
        bool MakeIteration(TSourceDataCollection sourceDatas, TChainSmallOut oldPrivateOut, out TChainSmallIn privateIn);
        TRepeaterBigOut Conclude();
    }

    public interface IRepeaterBlock<TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut>
    {
        void Start(TRepeaterBigIn publicIn);
        bool MakeIteration(TChainSmallOut oldPrivateOut, out TChainSmallIn privateIn);
        TRepeaterBigOut Conclude();
    }
}