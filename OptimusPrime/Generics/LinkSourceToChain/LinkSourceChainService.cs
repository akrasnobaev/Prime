using Eurobot.Services;
using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Generics
{
    public class LinkSourceToChainService<TInput,TOutput> : IGenericService
    {
        Func<TInput, TOutput> process;
        public readonly SourceBlock<TOutput> SourceBlock = new SourceBlock<TOutput>();
        ISource<TInput> input;
        ISourceReader<TInput> reader;

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
