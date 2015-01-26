using System;
using System.Collections.Generic;
using Prime.Liberty;

namespace Prime
{
    public partial class LibertyFactory
    {
        public override ISource<T> CreateSource<T>(IEventBlock<T> eventBlock, string pseudoName = null)
        {
            var collectionName = ServiceNameHelper.GetCollectionName<T>();

            //Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, collectionName);

            var callSource = new LibertySource<T>(this, collectionName);
            var smartClone = new SmartClone<T>();

            OPEventHandler<T> sourceBlockOnEvent = (sender, inputData) =>
            {
                callSource.Collection.Add(smartClone.Clone(inputData));
                callSource.Release();
            };

            // Добавляем коллекцию источника в список логируемых коллекций и логируем временные отпечатки,
            // если логирование включено.
            if (IsLogging)
            {
                var timestampCollection = new List<TimeSpan>();

                collections.Add(collectionName, callSource.Collection);
                timestamps.Add(collectionName, timestampCollection);

                sourceBlockOnEvent = (sender, inputData) =>
                {
                    callSource.Collection.Add(smartClone.Clone(inputData));
                    // Логирование времени получения данных.
                    timestampCollection.Add(Stopwatch.Elapsed);
                    callSource.Release();
                };
            }

            eventBlock.Event += sourceBlockOnEvent;
            return callSource;
        }
    }
}