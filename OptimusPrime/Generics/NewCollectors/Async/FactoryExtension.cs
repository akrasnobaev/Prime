using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Generics
{
    public static partial class FactoryExtension
    {
        public static AsyncCollector<T> CreateCollector<T>(this ISource<T> source)
        {
            var reader = source.CreateReader();
            return new AsyncCollector<T>(source.Factory.CreateChain<Token, T[]>(z => reader.GetCollection()));

        }
    }
}
