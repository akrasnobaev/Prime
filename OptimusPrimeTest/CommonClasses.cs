using Eurobot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OptimusPrimeTests
{
    public class DataProducer<T>
    {
        public readonly SourceBlock<T> SourceBlock = new SourceBlock<T>();
        public void Start(int count, int rate, Func<int, T> data)
        {
            for (int i = 0; i < count; i++)
            {
                SourceBlock.Publish(data(i));
                Thread.Sleep(rate);
            }
        }
    }
}
