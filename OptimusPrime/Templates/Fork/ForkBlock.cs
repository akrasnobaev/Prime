using System;

namespace Prime
{
    public class ForkBlock<T, Q>
    {
        public Func<T, Q> ForkedAction { get; private set; }
        public EventBlock<Q> Event { get; private set; }

        public ForkBlock(Func<T, Q> baseAction)
        {
            Event = new EventBlock<Q>();
            ForkedAction = (
                z =>
                {
                    var result = baseAction(z);
                    Event.Publish(result);
                    return result;
                });
        }
    }
}