using Eurobot.Services;
using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Generics
{
    public class LinkToEnumerableService<TInput,TOutput> : IGenericService
    {
        Func<TInput, IEnumerable<TOutput>> process;
        public readonly SourceBlock<TOutput> SourceBlock = new SourceBlock<TOutput>();
        ISource<TInput> input;
        ISourceReader<TInput> reader;
        
        public LinkToEnumerableService(ISource<TInput> input, Func<TInput,IEnumerable<TOutput>> process)
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
