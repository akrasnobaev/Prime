using System;
using System.Collections.Generic;

namespace Prime
{
    public class LinkSourceToEnumerableService<TInput, TOutput> : IGenericService
    {
        private Func<TInput, IEnumerable<TOutput>> process;
        public readonly EventBlock<TOutput> EventBlock = new EventBlock<TOutput>();
        private IReciever<TInput> input;
        private IReader<TInput> reader;

        public LinkSourceToEnumerableService(ISource<TInput> input, Func<TInput, IEnumerable<TOutput>> process)
        {
            this.input = input.Factory.CreateReciever(input);
            this.process = process;
        }

        public void Initialize()
        {
            reader = input.GetReader();
        }

        public void DoWork()
        {
            while (true)
            {
                var data = reader.Get();
                foreach (var e in process(data))
                    EventBlock.Publish(e);
            }
        }
    }
}