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
        public static void Listen<TInput>
            (this ISource<TInput> input, Action<TInput> process)
        {
            var service = new ListenerService<TInput>(input, process);
            input.Factory.RegisterGenericService(service);
        }

       
    }
}
