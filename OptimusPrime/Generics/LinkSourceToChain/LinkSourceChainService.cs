using System;

namespace Prime
{
    public class LinkSourceToChainService<TInput, TOutput> : IGenericService
    {
        private Func<TInput, TOutput> process;
        public readonly SourceBlock<TOutput> SourceBlock = new SourceBlock<TOutput>();
        private ISource<TInput> input;
        private ISourceReader<TInput> reader;

        public LinkSourceToChainService(ISource<TInput> input, Func<TInput, TOutput> process)
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
                SourceBlock.Publish(process(reader.Get()));
            }
        }
    }
}