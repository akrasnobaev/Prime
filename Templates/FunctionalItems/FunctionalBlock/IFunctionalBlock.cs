namespace OptimusPrime.Templates
{
    public interface IFunctionalBlock<TIn, TOut>
    {
        TOut Process(TIn input);
    }
}