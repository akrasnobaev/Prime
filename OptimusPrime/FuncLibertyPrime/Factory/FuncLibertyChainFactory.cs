using System;
using System.Collections.Generic;
using Prime.Liberty;

namespace Prime
{
    public class FuncLibertyFactory : LibertyFactory
    {
        public override IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> function, string pseudoName = null)
        {
            var outputName = ServiceNameHelper.GetCollectionName<TOut>();
            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            var clone = new SmartClone<TOut>();
            var process = new Func<TIn, TOut>(inputData => clone.Clone(function(inputData)));

            // Добавляем коллекцию для логирования только в том случае, если логирование включено.
            if (IsLogging)
            {
                var logCollection = new PrintableList<object>();
                var timestampCollection = new List<TimeSpan>();

                collections.Add(outputName, logCollection);
                timestamps.Add(outputName, timestampCollection);

                process = inputData =>
                {
                    logCollection.Add(inputData);
                    timestampCollection.Add(Stopwatch.Elapsed);
                    return clone.Clone(function(inputData));
                };
            }

            return new LibertyChain<TIn, TOut>(this, process, outputName);
        }

        public override IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first,
            IChain<TMiddle, TOut> second)
        {
            // Помечаем исходные цепочки как использованные.
            first.MarkUsed();
            second.MarkUsed();

            // Используем небезопасное кастование, чтобы исключение указало на правильное место.
            var firstLibertyChain = (ILibertyChain<TIn, TMiddle>) first;
            var secondLibertyChain = (ILibertyChain<TMiddle, TOut>) second;

            var process = new Func<TIn, TOut>(dataInput => secondLibertyChain.Action(firstLibertyChain.Action(dataInput)));
            return new LibertyChain<TIn, TOut>(this, process, second.OutputName);
        }
    }
}