namespace OptimusPrime.Templates
{
    public interface IRepeaterBlock<TRepeaterBigIn, TRepeaterBigOut, TChainSmallIn, TChainSmallOut, TSourceDataCollection>
    {
        void Start(TRepeaterBigIn publicIn);
        bool MakeIteration(TSourceDataCollection sourceDatas, TChainSmallOut oldPrivateOut, out TChainSmallIn privateIn);
        //todo: не передавть TPrivateOut
        TRepeaterBigOut Conclude();
    }
}