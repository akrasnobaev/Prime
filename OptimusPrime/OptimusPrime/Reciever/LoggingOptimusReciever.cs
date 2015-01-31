using System.Diagnostics;

namespace Prime.Optimus
{
    public class LoggingOptimusReciever<TIn> : OptimusReciever<TIn>
    {
        public IPrimeFactory Factory { get; private set; }

        public LoggingOptimusReciever(PrimeFactory factory, IOptimusIn input, string readLogName):
            base(factory, input, readLogName)
        {
            ReadLogName = readLogName;
            readLog = new OptimusOut(readLogName, new OptimusStabService(), new Stopwatch(), true);
        }

        //todo hide somehow?
        public override TIn Get()
        {
            var res = base.Get();
            readLog.Set(new object[]{res});
            return res;
        }

        public override TIn[] GetCollection()
        {
            var res = base.GetCollection();
            readLog.Set(res);
            return res;
        }

        public override bool TryGet(out TIn data)
        {
            var res = base.TryGet(out data);
            readLog.Set(new object[]{data});
            return res;
        }

        public string ReadLogName { get; private set; }

        private IOptimusOut readLog { get; set; }
    }
}
