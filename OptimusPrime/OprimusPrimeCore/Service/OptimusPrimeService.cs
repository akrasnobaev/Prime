using BookSleeve;

namespace OptimusPrime.OprimusPrimeCore
{
    public abstract class OptimusPrimeService : IOptimusPrimeService
    {
        protected OptimusPrimeService(string host = "localhost", int dbPage = 1)
        {
            DbPage = dbPage;

            Connection = new RedisConnection(host);

            var openTask = Connection.Open();
            Connection.Wait(openTask);
        }

        ~OptimusPrimeService()
        {
            Connection.Close(true);
        }

        public RedisConnection Connection { get; private set; }
        public int DbPage { get; private set; }
        public virtual string Name { get { return GetType().ToString(); } }
        public IOptimusPrimeIn[] OptimusPrimeIn { get; protected internal set; }
        public IOptimusPrimeOut[] OptimusPrimeOut { get; protected internal set; }

        public abstract void Initialize();
        public abstract void DoWork();
    }
}