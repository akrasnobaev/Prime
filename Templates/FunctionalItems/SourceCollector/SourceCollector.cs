using System.Collections.Generic;

namespace OptimusPrime.Templates
{
    public class SourceCollector<TDataCollection> : ISourceCollector<TDataCollection>
        where TDataCollection : ISourceDataCollection, new()
    {
        private readonly ISourceReader[] sourceReaders;

        public SourceCollector(ISourceReader[] sourceReaders)
        {
            this.sourceReaders = sourceReaders;
        }

        public TDataCollection Get()
        {
            var collection = new TDataCollection();
            for (int i = 0; i < collection.ListCount; i++)
                if (sourceReaders[i]!=null) //костыль? Сделать флаг в reader?
                    collection.Pull(i, sourceReaders[i].GetCollectionNonTypized());
            return collection;
            
        }
    }
}