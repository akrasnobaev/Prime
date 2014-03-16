using System.Diagnostics;

namespace Prime.Optimus
{
    public class OptimusOut : IOptimusOut
    {
        private readonly Stopwatch stopwatch;
        private readonly bool isLogging;
        private readonly string timeStampsName;
        public string Name { get; private set; }
        public IOptimusService Service { get; private set; }

        public OptimusOut(string storageKey, IOptimusService service, Stopwatch stopwatch, bool isLogging)
        {
            this.stopwatch = stopwatch;
            this.isLogging = isLogging;
            timeStampsName = ServiceNameHelper.GetTimeStampName(storageKey);
            Name = storageKey;
            Service = service;
        }

        public void Set(object value)
        {
            var valueBytes = value.Serialize();
            var taskAdd = Service.Connection.Lists.AddLast(Service.DbPage, Name, valueBytes);
            Service.Connection.Wait(taskAdd);
            
            // Логируем отпечаток времени только если включено логирование.
            if (isLogging)
            {
                var timeStampBytes = stopwatch.Elapsed.Serialize();
                var taskTimeStamp = Service.Connection.Lists.AddLast(Service.DbPage, timeStampsName, timeStampBytes);
                Service.Connection.Wait(taskTimeStamp);
            }

            var taskPublish = Service.Connection.Publish(Name, "");
            Service.Connection.Wait(taskPublish);
        }
    }
}