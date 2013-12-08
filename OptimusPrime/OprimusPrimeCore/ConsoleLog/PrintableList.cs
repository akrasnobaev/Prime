using System;
using System.Collections.Generic;

namespace OptimusPrime.OprimusPrimeCore.ConsoleLog
{
    public class PrintableList<T> : List<T>
    {
        public bool IsPrint { get; private set; }

        public delegate string ToString(T item);

        private ToString _toString;

        public PrintableList() : base()
        {
            IsPrint = false;
            _toString = item => item.ToString();
        }

        public void Print(ToString toString)
        {
            IsPrint = true;
            _toString = toString;
        }

        public void UnPrint()
        {
            IsPrint = false;
        }

        public new void Add(T item)
        {
            base.Add(item);
            if (IsPrint)
                Console.WriteLine(_toString(item));
        }
    }
}