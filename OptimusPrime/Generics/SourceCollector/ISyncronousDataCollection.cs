using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Templates
{
    public interface ISyncronousDataCollection
    {
        int FieldsCount { get; }
        void GetOne(int index, object data);
    }
}
