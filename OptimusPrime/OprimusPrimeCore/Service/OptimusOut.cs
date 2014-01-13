namespace Prime.Optimus
{
    public class OptimusOut : IOptimusOut
    {
        public string Name { get; private set; }
        public IOptimusService Service { get; private set; }

        public OptimusOut(string storageKey, IOptimusService service)
        {
            Name = storageKey;
            Service = service;
        }

        public void Set(object value)
        {
            var bytes = value.Serialize();

            var taskAdd = Service.Connection.Lists.AddLast(Service.DbPage, Name, bytes);
            Service.Connection.Wait(taskAdd);

            var taskPublish = Service.Connection.Publish(Name, "");
            Service.Connection.Wait(taskPublish);
        }
    }
}