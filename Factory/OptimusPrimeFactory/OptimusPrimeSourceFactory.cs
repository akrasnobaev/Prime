using OptimusPrime.OprimusPrimeCore.Helpers;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class OptimusPrimeFactory
    {
        //todo: CreateSource by Chain
        public ISource<TPublic> CreateSource<TPublic>(ISourceBlock<TPublic> sourceBlock)
        {
            string outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusPrimeSourceService<TPublic>(sourceBlock, outputName);

            Services.Add(service);

            return new OptimusPrimeSource<TPublic>(service.Output);
        }

        public ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> _source,
                                                                 IChain<T1, T2> _chain)
        {
            var chain = _chain as IOptimusPrimeChane<T1, T2>;
            var source = _source as IOptimusPrimeSource<T1>;
            chain.Input.ChangeName(source.Output.Name);

            return new OptimusPrimeSource<T2>(chain.Output);
        }
    }
}