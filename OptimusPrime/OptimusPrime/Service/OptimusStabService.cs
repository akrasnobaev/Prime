namespace Prime.Optimus
{
    public class OptimusStabService : OptimusService
    {
        public OptimusStabService(string host = "localhost", int dbPage = 1) : base(host, dbPage)
        {
            OptimusIn = new IOptimusIn[0];
            OptimusOut = new IOptimusOut[0];
        }

        public override void Initialize()
        {
        }

        public override void DoWork()
        {
        }
    }
}