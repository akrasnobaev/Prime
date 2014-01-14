namespace Prime.Optimus
{
    public interface IOptimusChane<TIn, TOut> : IChain<TIn, TOut>
    {
        IOptimusIn Input { get; }
        IOptimusOut Output { get; }
    }
}