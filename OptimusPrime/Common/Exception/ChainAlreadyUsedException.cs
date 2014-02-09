using Prime;

namespace OptimusPrime.Common.Exception
{
    public class ChainAlreadyUsedException : PrimeException
    {
        public ChainAlreadyUsedException() : base("Ошибка при попытке дважды использовать цепочку." +
                                                  "Может возникнуть при повторном вызове метода ToFunctionalBlock() " +
                                                  "у одной и той же цепочки.")
        {
        }
    }
}