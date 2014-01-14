namespace Prime.Optimus
{
    public class OptimusSource<TPublic> : IOptimusSource<TPublic>
    {
        public IPrimeFactory Factory { get; private set; }

        public OptimusSource(PrimeFactory factory, IOptimusOut output)
        {
            Factory = factory;
            Output = output;
        }

        public IOptimusOut Output { get; private set; }

        public ISourceReader<TPublic> CreateReader()
        {
            return new OptimusSourceReader<TPublic>(Output.Name);
        }

        public string Name
        {
            get { return Output.Name; }
        }
    }
}