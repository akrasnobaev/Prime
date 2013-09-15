using System.Collections.Generic;
using System.Threading;

namespace OptimusPrime.Templates
{
    public interface ICallSource <T> : ISource<T>
    {
        IList<T> Collection { get; }

        /// <summary>
        /// Уведомляет все SourceReader о возможности чтения из источника.
        /// </summary>
        void Release();
    }
}