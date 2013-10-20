using System.Diagnostics;
using System.Threading;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class CallFactory
    {
        public ISource<T> CreateSource<T>(ISourceBlock<T> sourceBlock)
        {
            var collectionName = GetCollectionName<T>();
            var callSource = new CallSource<T>(this, collectionName);

            sourceBlock.Event += (sender, e) =>
                {
                    callSource.Collection.Add(e);
                    callSource.Release();
                };

            _collections.Add(collectionName, callSource.Collection);
            return callSource;
        }

        public ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> source, IChain<T1, T2> chain)
        {
            var collectionName = GetCollectionName<T2>();
            var newSource = new CallSource<T2>(this, collectionName);
            var startSuccesed = new AutoResetEvent(false);

            var newSourceThread = new Thread(() =>
                {
                    var sourceReader = source.CreateReader();
                    startSuccesed.Set();

                    while (true)
                    {
                        T1 inputData = sourceReader.Get();
                        T2 outputData = ((ICallChain<T1, T2>) chain).Action(inputData);

                        newSource.Collection.Add(outputData);
                        newSource.Release();
                    }
                });

            _collections.Add(collectionName, newSource.Collection);
            _threads.Add(newSourceThread);
            _threadsStartSuccessed.Add(startSuccesed);

            return newSource;
        }
    }
}