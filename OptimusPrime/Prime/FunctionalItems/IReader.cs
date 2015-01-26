using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime
{
    public interface IReader<T>
    {
        T Get();
        bool TryGet(out T data);
        T[] GetCollection();
    }
}
