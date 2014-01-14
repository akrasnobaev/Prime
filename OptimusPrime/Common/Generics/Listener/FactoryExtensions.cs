using System;

namespace Prime
{
    public static partial class FactoryExtensions
    {
        public static void Listen<TInput>
            (this ISource<TInput> input, Action<TInput> process)
        {
            var service = new ListenerService<TInput>(input, process);
            input.Factory.RegisterGenericService(service);
        }
    }
}