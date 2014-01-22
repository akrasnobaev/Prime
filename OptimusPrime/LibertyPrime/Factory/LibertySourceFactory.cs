using System;
using System.Collections.Generic;
using Prime.Liberty;

namespace Prime
{
    public partial class LibertyFactory
    {
        public override ISource<T> CreateSource<T>(ISourceBlock<T> sourceBlock, string pseudoName = null)
        {
            var collectionName = ServiceNameHelper.GetCollectionName<T>();
            
            //Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, collectionName);

            var callSource = new LibertySource<T>(this, collectionName);
            var timestampCollection = new List<TimeSpan>();
            var smartClone = new SmartClone<T>();
            sourceBlock.Event += (sender, inputData) =>
            {
                callSource.Collection.Add(smartClone.Clone(inputData));
                // Логирование времени получения данных.
                timestampCollection.Add(Stopwatch.Elapsed);
                callSource.Release();
            };

            collections.Add(collectionName, callSource.Collection);
            timestamps.Add(collectionName, timestampCollection);
            return callSource;
        }
    }
}