namespace OptimusPrime.Templates
{
    public interface IChain<TIn, TOut>
    {
        ICallChain<TIn, TOut> ToCallChain();
    }
}