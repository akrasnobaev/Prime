using System.Threading;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class CallFactory
    {
        public ISource<T> CreateSource<T>(ISourceBlock<T> sourceBlock)
        {
            var callSource = new CallSource<T>();
            sourceBlock.Event += (sender, args) =>
                {
                    callSource.Collection.Add(args.Data);
                    // callSource.AutoResetEvent.Set(); ??
                };
            return callSource;
        }

        public ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> source, IChain<T1, T2> chain)
        {
            var newSource = new CallSource<T2>();
            var newSourceThread = new Thread(() =>
                {
                    var sourceReader = source.CreateReader();
                    while (true)
                    {
                        T1 inputData = sourceReader.Get();
                        T2 outputData = (chain as ICallChain<T1,T2>).Action(inputData);
                        newSource.Collection.Add(outputData);
                    }
                });
            threads.Add(newSourceThread);
            return newSource;
        }
    }
}