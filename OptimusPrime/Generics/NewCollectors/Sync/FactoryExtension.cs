using OptimusPrime.Generics;
using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Factory
{
    public static partial class FactoryExtension
    {
        public static SyncCollector<T> CreateSyncCollector<T>(this ISource<T> source)
        {
            var reader = source.CreateReader();
            return new SyncCollector<T>(source.Factory.CreateChain<Token, T>(z => reader.Get()));

        }
    }
}
