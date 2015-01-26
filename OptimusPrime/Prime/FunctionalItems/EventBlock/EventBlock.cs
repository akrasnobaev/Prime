namespace Prime
{
    public class EventBlock<T> : IEventBlock<T>
    {
        public void Publish(T value)
        {
            if (Event != null)
                Event(this, value);
        }

        public event OPEventHandler<T> Event;
    }
}