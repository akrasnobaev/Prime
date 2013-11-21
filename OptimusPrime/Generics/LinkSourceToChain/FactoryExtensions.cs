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
        //public static ISource<TOutput> Link<TInput,TOutput>
        //    (this ISource<TInput> input, Func<TInput,TOutput> process, string pseudoname=null)
        //{
        //    var service = new LinkSourceToChainService<TInput, TOutput>(input, process);
        //    input.Factory.RegisterGenericService(service);
        //    return input.Factory.CreateSource(service.SourceBlock, pseudoname);
        //}

        //public static ISource<TOutput> Link<TInput, TOutput>
        //   (this ISource<TInput> input, IFunctionalBlock<TInput, TOutput> process, string pseudoname = null)
        //{
        //    return Link(input, process.Process, pseudoname);
        //}
       
    }
}
