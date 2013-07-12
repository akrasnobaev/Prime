using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class OptimusPrimeFactory
    {
        //todo: CreateSource by Chain
        public IOptimusPrimeSource<TPublic> CreateSource<TPublic>(ISourceBlock<TPublic> sourceBlock)
        {
            string outputName = string.Format("{0}_out", sourceBlock.GetType().Name);
            var service = new OptimusPrimeSourceService<TPublic>(sourceBlock, outputName);

            Services.Add(service);

            return new OptimusPrimeSource<TPublic>(service.Output);
        }

        public IOptimusPrimeSource<T2> LinkSourceToChain<T1, T2>(IOptimusPrimeSource<T1> source,
                                                                 IOptimusPrimeChane<T1, T2> chain)
        {
            chain.Input.ChangeName(source.Output.Name);

            return new OptimusPrimeSource<T2>(chain.Output);
        }
    }
}