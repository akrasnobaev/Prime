using System;
using System.Collections.Generic;
using System.Threading;
using Prime.Liberty;

namespace Prime
{
    public partial class LibertyFactory
    {
        public ISource<T> CreateSource<T>(ISourceBlock<T> sourceBlock, string pseudoName = null)
        {
            var collectionName = GetCollectionName<T>();
            
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

        public ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> source, IChain<T1, T2> chain, string pseudoName = null)
        {
            var callChain = (ILibertyChain<T1, T2>) chain;
            callChain.SetInputName(source.Name);
            var newSource = new LibertySource<T2>(this, callChain.OutputName);
            var startSuccesed = new AutoResetEvent(false);

            //Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, callChain.OutputName);
            
            var newSourceThread = new Thread(() =>
            {
                var sourceReader = source.CreateReader();
                var smartClone = new SmartClone<T2>();
                startSuccesed.Set();

                while (true)
                {
                    T1 inputData = sourceReader.Get();
                    T2 outputData = callChain.Action(inputData);

                    newSource.Collection.Add(smartClone.Clone(outputData));
                    newSource.Release();
                }
            });

            threads.Add(newSourceThread);
            threadsStartSuccessed.Add(startSuccesed);

            return newSource;
        }

        public ISource<T> LinkSourceToFilter<T>(ISource<T> source, IFunctionalBlock<T, bool> filterBlock,
            string pseudoName = null)
        {
            var collectionName = GetCollectionName<T>();
            var newSource = new LibertySource<T>(this, collectionName);
            var startSuccesed = new AutoResetEvent(false);

            //Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, collectionName);

            var newSourceThread = new Thread(() =>
            {
                var sourceReader = source.CreateReader();
                var smartClone = new SmartClone<T>();
                startSuccesed.Set();

                while (true)
                {
                    T inputData = sourceReader.Get();
                    var filteringResult = filterBlock.Process(inputData);
                    if (!filteringResult) continue;
                    newSource.Collection.Add(smartClone.Clone(inputData));
                    newSource.Release();
                }
            });

            threads.Add(newSourceThread);
            threadsStartSuccessed.Add(startSuccesed);

            return newSource;
        }
    }
}