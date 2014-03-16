using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Prime
{
    public abstract class PrimeFactoryBase : IPrimeFactory
    {
        public abstract string DumpDb();
        public abstract IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> function, string pseudoName = null);
        public abstract ISource<TData> CreateSource<TData>(ISourceBlock<TData> block, string pseudoName = null);
        public abstract void ConsoleLog<T>(string InputName, PrintableList<T>.ToString ToString = null);

        protected PrimeFactoryBase(bool isLogging = true)
        {
            Stopwatch = new Stopwatch();
            threads = new List<Thread>();
            threadsStartSuccessed = new List<AutoResetEvent>();
            IsLogging = isLogging;
        }

        public virtual void Start()
        {
            // Стартуем секундомер, определяющий момент возникновения данных.
            Stopwatch.Start();

            foreach (var thread in threads)
                thread.Start();

            // Ожидание того, что все потоки стартовали успешно.
            foreach (var resetEvent in threadsStartSuccessed)
                resetEvent.WaitOne();
        }

        public virtual void Stop()
        {
            foreach (var thread in threads)
                thread.Abort();

            Stopwatch.Stop();
        }

        public virtual IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first,
            IChain<TMiddle, TOut> second)
        {
            var firstBlock = first.ToFunctionalBlock();
            var secondBlock = second.ToFunctionalBlock();
            return CreateChain<TIn, TOut>(input =>
            {
                TMiddle middle = firstBlock.Process(input);
                TOut output = secondBlock.Process(middle);
                return output;
            });
        }

        public virtual ISource<TOut> LinkSourceToChain<TIn, TOut>(ISource<TIn> source, IChain<TIn, TOut> chain,
            string pseudoName = null)
        {
            var sourceBlock = new SourceBlock<TOut>();
            var genericService = new LinkSourceToChainGenericService<TIn, TOut>(source, chain, sourceBlock);
            RegisterGenericService(genericService);
            return CreateSource(sourceBlock, pseudoName);
        }

        public virtual ISource<T> LinkSourceToFilter<T>(ISource<T> source, IFunctionalBlock<T, bool> filterBlock,
            string pseudoName = null)
        {
            var sourceBlock = new SourceBlock<T>();
            var genericService = new LinkSourceToFilterGenericService<T>(source, filterBlock, sourceBlock);
            RegisterGenericService(genericService);
            return CreateSource(sourceBlock, pseudoName);
        }

        public void RegisterGenericService(IGenericService service)
        {
            var startSuccesed = new AutoResetEvent(false);
            var serviceThread = new Thread(() =>
            {
                service.Initialize();
                startSuccesed.Set();
                service.DoWork();
            });
            threads.Add(serviceThread);
            threadsStartSuccessed.Add(startSuccesed);
        }

        /// <summary>
        /// Секундомер, стартующий вместе со стартом фабрики
        /// </summary>
        public Stopwatch Stopwatch { get; private set; }

        public bool IsLogging { get; private set; }

        /// <summary>
        /// Коллекция потоков, содержащих все сервисы.
        /// </summary>
        protected IList<Thread> threads { get; private set; }

        /// <summary>
        /// Коллекция AutoResetEvent, которые релизятся в тот момент,
        /// когда соответствующий поток успешно стартовал.
        /// </summary>
        protected IList<AutoResetEvent> threadsStartSuccessed { get; private set; }
    }
}