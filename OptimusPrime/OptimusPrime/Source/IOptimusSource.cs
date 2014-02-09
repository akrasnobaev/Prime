namespace Prime.Optimus
{
    public interface IOptimusSource<TPublic> : ISource<TPublic>
    {
        IOptimusOut Output { get; }
    }
}