using System.Threading;
using OptimusPrime.Templates.FunctionalItems.FunctionalBlock;
using OptimusPrime.Templates.FunctionalItems.SourceReader;
using OptimusPrime.Templates.TopologyItems.Chain.Call;
using OptimusPrime.Templates.TopologyItems.Source.Call;

namespace OptimusPrime.Factory.CallFactory
{
    public partial class CallFactory
    {
        public ICallSource<T> CreateSource<T>(ISourceBlock<T> sourceBlock)
        {
            var callSource = new CallSource<T>();
            sourceBlock.Event += (sender, args) => callSource.Collection.Add(args.Data);
            return callSource;
        }

        public ICallSource<T2> LinkSourceToChain<T1, T2>(ICallSource<T1> source, ICallChain<T1, T2> chain)
        {
            var newSource = new CallSource<T2>();
            var newSourceThread = new Thread(() =>
                {
                    var sourceReader = new SourceReader<T1>(source);
                    while (true)
                    {
                        T1 inputData = sourceReader.Get();
                        T2 outputData = chain.Action(inputData);
                        newSource.Collection.Add(outputData);
                    }
                });
            threads.Add(newSourceThread);
            return newSource;
        }
    }
}