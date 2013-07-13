namespace OptimusPrime.Templates
{
    public interface IChain<TIn, TOut>
    {
        IFunctionalBlock<TIn, TOut> ToFunctionalBlock();
    }
}