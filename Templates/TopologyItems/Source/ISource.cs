namespace OptimusPrime.Templates
{
    public interface ISource<TPublic>
    {
        ISourceReader<TPublic> CreateReader();
    }
}