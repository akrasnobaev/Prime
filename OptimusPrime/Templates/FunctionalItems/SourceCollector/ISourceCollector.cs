namespace OptimusPrime.Templates
{
    public interface ISourceCollector<TDataCollection> where TDataCollection : ISourceDataCollection
    {
        TDataCollection Get();
    }
}