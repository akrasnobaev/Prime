using Eurobot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Factory
{
    public class ForkBlock<T, Q>
    {
        public Func<T, Q> ForkedAction { get; private set; }
        public SourceBlock<Q> Source { get; private set; }
        public ForkBlock(Func<T, Q> baseAction)
        {
            Source = new SourceBlock<Q>();
            ForkedAction = new Func<T, Q>(
                z =>
                {
                    var result = baseAction(z);
                    Source.Publish(result);
                    return result;
                });
        }
    }
}
