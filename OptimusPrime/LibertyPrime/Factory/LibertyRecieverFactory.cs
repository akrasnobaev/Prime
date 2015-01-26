using System;

namespace Prime
{
    public partial class LibertyFactory
    {
        public override IReciever<TOut> CreateReciever<TOut>(ISource<TOut> source)
        {
            throw new NotImplementedException();
        }
    }
}
