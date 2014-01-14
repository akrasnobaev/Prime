using BookSleeve;

namespace Prime.Optimus
{
    public abstract class OptimusService : IOptimusService
    {
        protected OptimusService(string host = "localhost", int dbPage = 1)
        {
            DbPage = dbPage;

            Connection = new RedisConnection(host);

            var openTask = Connection.Open();
            Connection.Wait(openTask);
        }

        ~OptimusService()
        {
            Connection.Close(true);
        }

        public RedisConnection Connection { get; private set; }
        public int DbPage { get; private set; }

        public virtual string Name
        {
            get { return GetType().ToString(); }
        }

        public IOptimusIn[] OptimusIn { get; protected internal set; }
        public IOptimusOut[] OptimusOut { get; protected internal set; }

        public abstract void Initialize();
        public abstract void DoWork();
    }
}