﻿namespace Prime
{
    public class SourceBlock<T> : ISourceBlock<T>
    {
        public void Publish(T value)
        {
            if (Event != null)
                Event(this, value);
        }

        public event OPEventHandler<T> Event;
    }
}