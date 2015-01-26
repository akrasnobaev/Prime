using System;
using System.Collections.Generic;


namespace Prime
{
    public static partial class FactoryExtensions
    {
        public static ISource<TOutput> LinkToEnumerable<TInput, TOutput>
            (this ISource<TInput> input, Func<TInput, IEnumerable<TOutput>> process, string pseudoname = null)
        {
            var service = new LinkSourceToEnumerableService<TInput, TOutput>(input, process);
            input.Factory.RegisterGenericService(service);
            return input.Factory.CreateSource(service.EventBlock, pseudoname);
        }

        public static ISource<TOutput> LinkToEnumerable<TInput, TOutput>
            (this ISource<TInput> input, IFunctionalBlock<TInput, IEnumerable<TOutput>> process,
                string pseudoname = null)
        {
            return LinkToEnumerable(input, process.Process, pseudoname);
        }
    }
}