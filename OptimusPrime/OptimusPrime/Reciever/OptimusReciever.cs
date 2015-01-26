using System;
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
            ReadLogName = readLogName;
            readLog = new OptimusOut(readLogName, new OptimusStabService(), new Stopwatch(), true);
            reader = new OptimusReader<TIn>(this);
        }
        public IReader<TIn> GetReader()
        {
            return reader;
        }

        //todo hide somehow
        public TIn Get()
        {
            try
            {
                var item = Input.Get<TIn>();
                readLog.Set(new []{item});
                return item;
            }
            catch (PrimeException pe)
            {
                readLog.Set(new TIn[0]);
                throw pe;
            }
        }

        public bool TryGet(out TIn data)
        {
            var res = Input.TryGet(out data);
            readLog.Set(new[]{data});
            return res;
        }

        public TIn[] GetCollection()
        {
            var res = Input.GetRange<TIn>();
            readLog.Set(res);
            return res;
        }

        public string InputName { get { return Input.Name; } }

        public string ReadLogName { get; private set; }

        private IReader<TIn> reader; 

        public IOptimusIn Input { get; private set; }

        private IOptimusOut readLog { get; set; }
    }
}
