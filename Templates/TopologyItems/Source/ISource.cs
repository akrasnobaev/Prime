using OptimusPrime.Factory;
namespace OptimusPrime.Templates
{
    public interface ISource<TPublic>
    {
        IFactory Factory { get; }
        ISourceReader<TPublic> CreateReader();
    }
}