using OptimusPrime.Generics;
using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Factory
{
    public static partial class FactoryExtensions
    {
        public static ISource<TOutput> LinkToEnumerable<TInput,TOutput>
            (this ISource<TInput> input, Func<TInput,IEnumerable<TOutput>> process, string pseudoname=null)
        {
            var service = new LinkToEnumerableService<TInput, TOutput>(input, process);
            input.Factory.RegisterGenericService(service);
            return input.Factory.CreateSource(service.SourceBlock, pseudoname);
        }

        public static ISource<TOutput> LinkToEnumerable<TInput, TOutput>
           (this ISource<TInput> input, IFunctionalBlock<TInput, IEnumerable<TOutput>> process, string pseudoname = null)
        {
            return LinkToEnumerable(input, process.Process, pseudoname);
        }
       
    }
}
