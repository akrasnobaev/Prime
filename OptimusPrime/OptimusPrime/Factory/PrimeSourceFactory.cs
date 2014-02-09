using Prime.Optimus;

namespace Prime
{
    public partial class PrimeFactory
    {
        public override ISource<TPublic> CreateSource<TPublic>(ISourceBlock<TPublic> sourceBlock, string pseudoName = null)
        {
            string outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusSourceService<TPublic>(sourceBlock, outputName, Stopwatch);

            //Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            Services.Add(service);

            return new OptimusSource<TPublic>(this, service.Output);
        }

        public override ISource<T2> LinkSourceToChain<T1, T2>(ISource<T1> _source, IChain<T1, T2> _chain,
            string pseudoName = null)
        {
            // Помечаем цепочку как использованную.
            _chain.MarkUsed();

            var chain = _chain as IOptimusChane<T1, T2>;
            var source = _source as IOptimusSource<T1>;
            chain.Input.ChangeName(source.Output.Name);

            //Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, chain.Output.Name);

            return new OptimusSource<T2>(this, chain.Output);
        }

        public override ISource<T> LinkSourceToFilter<T>(ISource<T> source, IFunctionalBlock<T, bool> filterBlock,
            string pseudoName = null)
        {
            string outputName = ServiceNameHelper.GetOutName();
            var service = new OptimusFilterService<T>(filterBlock, source.Name, outputName, Stopwatch);

             //Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            Services.Add(service);

            return new OptimusSource<T>(this, service.Output);
        }
    }
}