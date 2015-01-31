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

        public IReciever<TPublic> CreateReciever(string readLogName = null)
        {
            return Factory.CreateReciever(this);
        }

        public string Name
        {
            get { return Output.Name; }
        }
    }
}