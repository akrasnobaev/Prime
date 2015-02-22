using System.Threading;

namespace Prime.Liberty
{
    //Todo really needed?
    public class LibertyReader<T> : IReader<T>
    {
        private readonly ILibertyReciever<T> reciever;

        public LibertyReader(ILibertyReciever<T> reciever)
        {
            this.reciever = reciever;
        }

        public T Get()
        {
            return reciever.Get();
        }

        public bool TryGet(out T data)
        {
            return reciever.TryGet(out data);
        }

        public T[] GetCollection()
        {
            return reciever.GetCollection();
        }
    }
}