﻿using System;

namespace Prime
{
    public static partial class FactoryExtensions
    {
        public static Fork<TIn, TOut> Fork<TIn, TOut>(this IChain<TIn, TOut> chain)
        {
            var forkBlock = new ForkBlock<TIn, TOut>(chain.ToFunctionalBlock().Process);
            return new Fork<TIn, TOut>(
                chain.Factory.CreateChain(forkBlock.ForkedAction),
                chain.Factory.CreateSource(forkBlock.Event));
        }
    }
}