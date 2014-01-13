using System;

namespace Prime.Liberty
{
    public class LibertyChain<T1, T2> : ILibertyChain<T1, T2>
    {
        public Func<T1, T2> Action { get; private set; }

        public void SetInputName(string inputName)
        {
            InputName = inputName;
        }

        public string InputName { get; private set; }
        public string OutputName { get; private set; }

        public LibertyChain(LibertyFactory factory, Func<T1, T2> action, string outputName)
        {
            Action = action;
            OutputName = outputName;
            Factory = factory;
        }

        public IFunctionalBlock<T1, T2> ToFunctionalBlock()
        {
            return new FunctionalBlock<T1, T2>(Action);
        }

        public IPrimeFactory Factory { get; private set; }
    }
}