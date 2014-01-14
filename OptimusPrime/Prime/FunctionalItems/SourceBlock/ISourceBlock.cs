namespace Prime
{
    public delegate void OPEventHandler<T>(object sender, T args);

    public interface ISourceBlock<T1>
    {
        event OPEventHandler<T1> Event;
    }
}