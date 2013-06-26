namespace OptimusPrime.Templates.FunctionalItems.SourceCollector
{
    public interface ISourceCollector<TDataCollection> where TDataCollection : ISourceDataCollection
    {
        TDataCollection Get();
    }
}