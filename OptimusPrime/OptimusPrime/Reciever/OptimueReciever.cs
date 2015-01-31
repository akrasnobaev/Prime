using System.Diagnostics;

namespace Prime.Optimus
{
    public class OptimusReciever<TIn> : IOptimusReciever<TIn>
    {
        public IPrimeFactory Factory { get; private set; }

        public OptimusReciever(PrimeFactory factory, IOptimusIn input, string readLogName)
        {
            Factory = factory;
            Input = input;
            reader = new OptimusReader<TIn>(this);
        }

        public IReader<TIn> GetReader()
        {
            return reader;
        }

        //todo hide somehow?
        public virtual TIn Get()
        {
            try
            {
                var item = Input.Get<TIn>();
                return item;
            }
            catch (PrimeException pe)
            {
                throw pe;
            }
        }

        public virtual bool TryGet(out TIn data)
        {
            var res = Input.TryGet(out data);
            return res;
        }

        public virtual TIn[] GetCollection()
        {
            var res = Input.GetRange<TIn>();
            return res;
        }

        public string InputName
        {
            get { return Input.Name; }
        }

        private IReader<TIn> reader;

        public IOptimusIn Input { get; private set; }
    }
}
