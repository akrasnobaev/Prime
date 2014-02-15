﻿using System;
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


    public class ByteArrayData : Data
    {
        public byte[] data;

        public ByteArrayData(int length) { data = new byte[length]; }
    }

    [ImmutableObject(true)]
    public class BigData : ByteArrayData
    {
        public BigData() : base(1000000) { }
    }

    [ImmutableObject(true)]
    public class MediumData : ByteArrayData
    {
        public MediumData() : base(10000) { }
    }

    [ImmutableObject(true)]
    public class SmallData : ByteArrayData
    {
        public SmallData() : base(100) { }
    }
}