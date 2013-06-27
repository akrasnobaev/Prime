using BookSleeve;

namespace OptimusPrime.OprimusPrimeCore.Service
{
    public abstract class OptimusPrimeService : IOptimusPrimeService
    {
        public RedisConnection Connection { get; set; }
        public int DbPage { get; set; }

        public virtual string Name { get { return GetType().ToString(); } }
        public IOptimusPrimeIn[] OptimusPrimeIn { get; protected set; }
        public IOptimusPrimeOut[] OptimusPrimeOut { get; protected set; }
        public abstract void Actuation();

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
    }
}