using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates.FunctionalItems.FunctionalBlock
{
    public class OptimusPrimeFilterService<T> : OptimusPrimeService
    {
        private readonly IFunctionalBlock<T, bool> filterBlock;
        private readonly OptimusPrimeIn input;
        public readonly OptimusPrimeOut Output;

        public OptimusPrimeFilterService(IFunctionalBlock<T, bool> filterBlock, string inputName,
            string outputName, string host = "localhost", int dbPage = 1) : base(host, dbPage)
        {
            this.filterBlock = filterBlock;
            input = new OptimusPrimeIn(inputName, this);
            Output = new OptimusPrimeOut(outputName, this);

            OptimusPrimeOut = new IOptimusPrimeOut[] {Output};
            OptimusPrimeIn = new IOptimusPrimeIn[] {input};
        }

        public override void Actuation()
        {
            while (true)
            {
                var data = input.Get<T>();
                if (!filterBlock.Process(data)) continue;
                Output.Set(data);
            }
        }
    }
}