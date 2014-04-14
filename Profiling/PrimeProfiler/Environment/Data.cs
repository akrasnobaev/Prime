using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    public class Data
    {
        public Data() { Marker = Rnd.Double(); }
        public double Marker;
    }


    public class ByteArrayData : Data, ICloneable
    {
        public byte[] data;

        public ByteArrayData(int length) { data = new byte[length]; }

        private ByteArrayData() { }

        public object Clone()
        {
            return new ByteArrayData { Marker = Marker, data = data };
        }
    }

    [ImmutableObject(true)]
    public class BigData : ByteArrayData
    {
        public BigData() : base(100) { }
    }

    [ImmutableObject(true)]
    public class MediumData : ByteArrayData
    {
        public MediumData() : base(1000) { }
    }

    [ImmutableObject(true)]
    public class SmallData : ByteArrayData
    {
        public SmallData() : base(10000) { }
    }
}
