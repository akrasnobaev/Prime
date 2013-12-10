using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Templates
{
    public class SyncSourceCollector<TDataCollection> : ISourceCollector<TDataCollection>
        where TDataCollection : ISyncronousDataCollection, new()
    {
        private readonly ISourceReader[] sourceReaders;
        
        public bool SkipFirstGet = true;
        bool firstGet = true;

        public void Reset()
        {
            firstGet = true;
        }

        public SyncSourceCollector(ISourceReader[] sourceReaders)
        {
            this.sourceReaders = sourceReaders;
        }

        public TDataCollection Get()
        {
            var collection = new TDataCollection();

            if (firstGet && SkipFirstGet)
            {
                firstGet = false;
                return collection;
            }
            
            for (int i = 0; i < collection.FieldsCount; i++)
                if (sourceReaders[i]!=null) //костыль? Сделать флаг в reader?
                    collection.GetOne(i, sourceReaders[i].GetNotTypized());
            return collection;
            
        }
    }
}
