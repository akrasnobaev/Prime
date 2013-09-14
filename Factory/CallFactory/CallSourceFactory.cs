using System.Threading;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class CallFactory
    {
        public ISource<T> CreateSource<T>(ISourceBlock<T> sourceBlock)
        {
            var callSource = new CallSource<T>(this);
            sourceBlock.Event += (sender, e) =>
                {
                    callSource.Collection.Add(e);
                    callSource.Semaphore.Release();
                };
            return callSource;
        }

        public ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> source, IChain<T1, T2> chain)
        {
            var newSource = new CallSource<T2>(this);
            var newSourceThread = new Thread(() =>
                {
                    ISourceReader<T1> sourceReader = source.CreateReader();
                    while (true)
                    {
                        T1 inputData = sourceReader.Get();
                        T2 outputData = ((ICallChain<T1, T2>) chain).Action(inputData);

                        newSource.Collection.Add(outputData);
                        newSource.Semaphore.Release();
                    }
                });
            threads.Add(newSourceThread);
            return newSource;
        }
    }
}