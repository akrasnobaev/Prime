using System;
using System.Collections.Generic;

namespace Prime
{
    public class LinkSourceToEnumerableService<TInput, TOutput> : IGenericService
    {
        private Func<TInput, IEnumerable<TOutput>> process;
        public readonly SourceBlock<TOutput> SourceBlock = new SourceBlock<TOutput>();
        private ISource<TInput> input;
        private ISourceReader<TInput> reader;

        public LinkSourceToEnumerableService(ISource<TInput> input, Func<TInput, IEnumerable<TOutput>> process)
        {
            this.input = input;
            this.process = process;
        }

        public void Initialize()
        {
            reader = input.CreateReader();
        }

        public void DoWork()
        {
            while (true)
            {
                var data = reader.Get();
                foreach (var e in process(data))
                    SourceBlock.Publish(e);
            }
        }
    }
}