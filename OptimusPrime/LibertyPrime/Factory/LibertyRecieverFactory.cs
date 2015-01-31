using Prime.Liberty;

namespace Prime
{
    public partial class LibertyFactory
    {
        public override IReciever<T> CreateReciever<T>(ISource<T> source, string readLogName = null)
        {
            if (readLogName == null)
                readLogName = ServiceNameHelper.GetCollectionName<T>();

            LibertyReciever<T> reciever; 
            if (IsLogging)
            {
                var loggigReciever = new LoggingLibertyReciever<T>(this, readLogName, collections[source.Name]);
                collections.Add(readLogName, loggigReciever.ReadLog);
                reciever = loggigReciever;
            }
            else
            {
                reciever = new LibertyReciever<T>(this, readLogName, (source as LibertySource<T>).Collection);
            }

            (source as LibertySource<T>).Recievers.Add(reciever); // todo bullshit, do something
            return reciever;
        }
    }
}
