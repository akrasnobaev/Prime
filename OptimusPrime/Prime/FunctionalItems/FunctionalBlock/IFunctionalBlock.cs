namespace Prime
{
    public interface IFunctionalBlock<TIn, TOut>
    {
        TOut Process(TIn input);
    }
}