﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurobot.Services;
using OptimusPrime.Templates;

namespace OptimusPrime.Factory
{
    public class Fork<T,Q>
    {
        public IChain<T, Q> Chain { get; private set; }
        public ISource<Q> Source { get; private set; }
        public Fork(IChain<T, Q> chain, ISource<Q> source)
        {
            Chain = chain;
            Source = source;
        }
    }

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
