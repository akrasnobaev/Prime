using System;
using OptimusPrime.Common.Exception;

namespace Prime.Liberty
{
    public class LibertyChain<T1, T2> : ILibertyChain<T1, T2>
    {
        private bool isUsed;

        public LibertyChain(IPrimeFactory factory, Func<T1, T2> action, string outputName)
        {
            Action = action;
            OutputName = outputName;
            Factory = factory;
            isUsed = false;
        }

        public Func<T1, T2> Action { get; private set; }

        public void SetInputName(string inputName)
        {
            InputName = inputName;
        }

        public string InputName { get; private set; }
        public string OutputName { get; private set; }

        public void MarkUsed()
        {
            if (isUsed)
                throw new ChainAlreadyUsedException();
            isUsed = true;
        }

        public IFunctionalBlock<T1, T2> ToFunctionalBlock()
        {
            MarkUsed();
            var smartClone = new SmartClone<T1>();
            return new FunctionalBlock<T1, T2>(data => Action(smartClone.Clone(data)));
        }

        public IPrimeFactory Factory { get; private set; }
    }
}