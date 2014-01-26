using Prime;

namespace OptimusPrime.Common.Exception
{
    public class LoggingOffException : PrimeException
    {
        public LoggingOffException()
            : base("Ошибка при попытке сделать дамп данных: в фабрике отключено логирование. " +
                   "Создайте фабрику с параметром `isLogging` = true, и повторите попытку.")
        {
        }
    }
}