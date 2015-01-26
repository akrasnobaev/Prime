namespace Prime
{
    public static partial class FactoryExtension
    {
        public static SyncCollector<T> CreateSyncCollector<T>(this ISource<T> source)
        {
            var reader = source.Factory.CreateReciever(source).GetReader();
            return new SyncCollector<T>(source.Factory.CreateChain<Token, T>(z => reader.Get()));
        }
    }
}