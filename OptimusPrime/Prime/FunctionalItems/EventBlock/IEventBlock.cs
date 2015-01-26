namespace Prime
{
    public delegate void OPEventHandler<T>(object sender, T args);

    public interface IEventBlock<T1>
    {
        event OPEventHandler<T1> Event;
    }
}