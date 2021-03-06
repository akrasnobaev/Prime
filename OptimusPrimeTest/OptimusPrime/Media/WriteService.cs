﻿using System.Collections.Generic;
using System.Diagnostics;
using OptimusPrimeTest.Prime;
using Prime.Optimus;

namespace OptimusPrimeTest.OptimusPrime
{
    public class WriteService<T> : OptimusService
    {
        private readonly IEnumerable<T> _testDatCollection;

        public WriteService(IEnumerable<T> testDatCollection)
            : base(TestConstants.Host, TestConstants.DbPage)
        {
            _testDatCollection = testDatCollection;
            OptimusOut = new IOptimusOut[] { new OptimusOut(TestConstants.StorageKey, this, new Stopwatch(), true) };
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
            foreach (T testData in _testDatCollection)
                OptimusOut[0].Set(testData);
        }
    }
}