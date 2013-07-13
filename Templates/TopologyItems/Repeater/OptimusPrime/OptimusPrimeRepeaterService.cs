using OptimusPrime.OprimusPrimeCore;

namespace OptimusPrime.Templates
{
    /*
    public class OptimusPrimeRepeaterService<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut, TDataCollection> : OptimusPrimeService where TDataCollection : ISourceDataCollection
    {
        private readonly IRepeaterBlock<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut> repeaterBlock;
        private readonly ISourceCollector<TDataCollection> sourceCollector;

        public IOptimusPrimeIn Input { get { return OptimusPrimeIn[0]; } }
        public IOptimusPrimeOut Output { get { return OptimusPrimeOut[0]; } }

        public OptimusPrimeRepeaterService(IRepeaterBlock<TPublicIn, TPrivateIn, TPublicOut, TPrivateOut> repeaterBlock,
                                           ISourceCollector<TDataCollection> sourceCollector, string publicInName,
                                           string privateInName, string publicOutName, string privateOutName)
        {
            this.repeaterBlock = repeaterBlock;
            this.sourceCollector = sourceCollector;

            OptimusPrimeIn = new IOptimusPrimeIn[]
                {
                    new OptimusPrimeIn(publicInName, this),
                    new OptimusPrimeIn(privateInName, this)
                };
            OptimusPrimeOut = new IOptimusPrimeOut[]
                {
                    new OptimusPrimeOut(publicOutName, this),
                    new OptimusPrimeOut(privateOutName, this)
                };
        }

        public override void Actuation()
        {
            while (true)
            {
                var publicIn = OptimusPrimeIn[0].Get<TPublicIn>();
                var privateIn = repeaterBlock.Start(publicIn);
                TPrivateOut privateOut;

                while (!repeaterBlock.MakeIteration(privateIn, sourceCollector.Get(), out privateOut))
                {
                    OptimusPrimeOut[1].Set(privateOut);
                    privateIn = OptimusPrimeIn[1].Get<TPrivateIn>();
                }

                var publicOut = repeaterBlock.Conclude(privateOut);
                OptimusPrimeOut[0].Set(publicOut);
            }
        }
    }
     * */
}