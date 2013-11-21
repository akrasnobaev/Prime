namespace OptimusPrime.Templates
{
    public interface ISourceCollector<TDataCollection>
    {
        TDataCollection Get();
    }
}