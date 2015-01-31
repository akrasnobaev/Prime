namespace Prime.Liberty
{
    public class LoggingLibertyReciever<T> : LibertyReciever<T>
    {
        public LoggingLibertyReciever(LibertyFactory factory, string inputName, PrintableList<object> collection) : 
            base(factory, inputName, collection)
        {
            ReadLog = new PrintableList<object>();
        }

        public override T Get()
        {
            var ret = base.Get();
            ReadLog.Add(new []{ret});
            return ret;
        }

        public override bool TryGet(out T data)
        {
            var res = base.TryGet(out data);
            ReadLog.Add(new []{data});
            return res;
        }

        public override T[] GetCollection()
        {
            var res = base.GetCollection();
            ReadLog.Add(res);
            return res;
        }

        public PrintableList<object> ReadLog { get; private set; }
    }
}
