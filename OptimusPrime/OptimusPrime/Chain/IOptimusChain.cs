namespace Prime.Optimus
{
    public interface IOptimusChain<TIn, TOut> : IChain<TIn, TOut>
    {
        IOptimusIn Input { get; }
        IOptimusOut Output { get; }
    }
}