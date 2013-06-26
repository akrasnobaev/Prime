using System.Collections.Generic;
using OptimusPrime.Templates.FunctionalItems.SourceReader;

namespace OptimusPrime.Templates.FunctionalItems.SourceCollector
{
    public class SourceCollector<TDataCollection, T1> : ISourceCollector<TDataCollection>
        where TDataCollection : ISourceDataCollection
    {
        private readonly ISourceDataCollectionFactory<TDataCollection, T1> sourceDataCollectionFactory;
        private readonly ISourceReader<T1> sourceReader1;

        public SourceCollector(ISourceReader<T1> sourceReader1,
                               ISourceDataCollectionFactory<TDataCollection, T1> sourceDataCollectionFactory)
        {
            this.sourceReader1 = sourceReader1;
            this.sourceDataCollectionFactory = sourceDataCollectionFactory;
        }

        public TDataCollection Get()
        {
            IEnumerable<T1> argument1 = sourceReader1.GetCollection();

            return sourceDataCollectionFactory.Create(argument1);
        }
    }
}