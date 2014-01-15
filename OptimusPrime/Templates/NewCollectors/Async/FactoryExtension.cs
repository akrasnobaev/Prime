namespace Prime
{
    public static partial class FactoryExtension
    {
        public static AsyncCollector<T> CreateAsyncCollector<T>(this ISource<T> source)
        {
            var reader = source.CreateReader();
            return new AsyncCollector<T>(source.Factory.CreateChain<Token, T[]>(z => reader.GetCollection()));
        }
    }
}