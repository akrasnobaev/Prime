using System.Threading;
using OptimusPrime.Templates;
using OptimusPrime.Generics;

namespace OptimusPrime.Factory
{
    public partial class CallFactory
    {
        public ISource<T> CreateSource<T>(ISourceBlock<T> sourceBlock, string pseudoName = null)
        {
            var collectionName = GetCollectionName<T>();
            var callSource = new CallSource<T>(this, collectionName);

            /**
             * Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
             */
            if (!string.IsNullOrEmpty(pseudoName))
                _pseudoNames.Add(pseudoName, collectionName);

            sourceBlock.Event += (sender, e) =>
                {
                    callSource.Collection.Add(e);
                    callSource.Release();
                };

            _collections.Add(collectionName, callSource.Collection);
            return callSource;
        }

        public ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> source, IChain<T1, T2> chain, string pseudoName = null)
        {
            var callChain = (ICallChain<T1, T2>) chain;
            callChain.SetInputName(source.Name);
            var newSource = new CallSource<T2>(this, callChain.OutputName);
            var startSuccesed = new AutoResetEvent(false);

            /**
             * Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
             */
            if (!string.IsNullOrEmpty(pseudoName))
                _pseudoNames.Add(pseudoName, callChain.OutputName);

            var newSourceThread = new Thread(() =>
                {
                    var sourceReader = source.CreateReader();
                    startSuccesed.Set();

                    while (true)
                    {
                        T1 inputData = sourceReader.Get();
                        T2 outputData = callChain.Action(inputData);

                        newSource.Collection.Add(outputData);
                        newSource.Release();
                    }
                });

            _threads.Add(newSourceThread);
            _threadsStartSuccessed.Add(startSuccesed);

            return newSource;
        }

        public ISource<T> LinkSourceToFilter<T>(ISource<T> source, IFunctionalBlock<T,bool> filterBlock, string pseudoName = null)
        {
            var collectionName = GetCollectionName<T>();
            var newSource = new CallSource<T>(this, collectionName);
            var startSuccesed = new AutoResetEvent(false);

            /**
             * Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
             */
            if (!string.IsNullOrEmpty(pseudoName))
                _pseudoNames.Add(pseudoName, collectionName);

            var newSourceThread = new Thread(() =>
            {
                var sourceReader = source.CreateReader();
                startSuccesed.Set();

                while (true)
                {
                    T inputData = sourceReader.Get();
                    var filteringResult = filterBlock.Process(inputData);
                    if (!filteringResult) continue;
                    newSource.Collection.Add(inputData);
                    newSource.Release();
                }
            });

            _threads.Add(newSourceThread);
            _threadsStartSuccessed.Add(startSuccesed);

            return newSource;
        }
    }
}