namespace OptimusPrime.Templates
{
    public interface IRepeaterBlock<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut>
    {
        TPrivateIn Start(TPublicIn publicIn);
        bool MakeIteration(TPrivateIn privateIn, ISourceDataCollection sourceDatas, out TPrivateOut privateOut);
        //todo: не передавть TPrivateOut
        TPublicOut Conclude(TPrivateOut privateOut);
    }
}