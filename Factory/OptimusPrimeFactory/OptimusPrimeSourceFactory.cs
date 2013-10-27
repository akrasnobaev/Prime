using OptimusPrime.OprimusPrimeCore.Helpers;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public partial class OptimusPrimeFactory
    {
        //todo: CreateSource by Chain
        public ISource<TPublic> CreateSource<TPublic>(ISourceBlock<TPublic> sourceBlock, string pseudoName = null)
        {
            string outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusPrimeSourceService<TPublic>(sourceBlock, outputName);

            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                _pseudoNames.Add(pseudoName, outputName);

            Services.Add(service);

            return new OptimusPrimeSource<TPublic>(this,service.Output);
        }

        public ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> _source,
                                                                 IChain<T1, T2> _chain)
        {
            var chain = _chain as IOptimusPrimeChane<T1, T2>;
            var source = _source as IOptimusPrimeSource<T1>;
            chain.Input.ChangeName(source.Output.Name);

            return new OptimusPrimeSource<T2>(this,chain.Output);
        }
    }
}