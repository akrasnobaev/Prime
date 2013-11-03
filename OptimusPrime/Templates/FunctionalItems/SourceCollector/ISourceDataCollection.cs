using System.Collections;
namespace OptimusPrime.Templates
{
    public interface ISourceDataCollection
    {
        int ListCount { get; }
        void Pull(int index, IEnumerable source);
    }

}