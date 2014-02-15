using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    abstract class TestSet
    {
        public int Length;
        public int Width;
        public int Waves;
        public Computations Comp;
        public Data Data;
        public TestSystem[] Tests;
        public abstract void Run();
        public static string PrintAll(TestSet[] set)
        {
            var builder = new StringBuilder();
            foreach (var e in set)
            {
                builder.AppendFormat("Length {0}     Width {1}     Waves {2}     Comp {3}     Data {4}\n",
                    e.Length,
                    e.Width,
                    e.Waves,
                    e.Comp.GetType().Name,
                    e.Data.GetType().Name);
                foreach (var w in e.Tests)
                {
                    builder.AppendFormat("{0,-25}{1}\n",
                        w.GetName(),
                        w.ElapsedMS);
                }
            }
            return builder.ToString();
        }

        public static void RunAll(TestSet[] set)
        {
            foreach (var e in set) e.Run();
        }

    }

    class TestSet<T> : TestSet
        where T : Data, new()
    {
        public TestSet(int Length, int Width, int Waves, Computations comp)
        {
            this.Length = Length;
            this.Waves = Waves;
            this.Width = Width;
            this.Comp = comp;
            Data = new T();
        }

        override public void Run()
        {
            Tests = new TestSystem[] 
            {
                new CCROneChain<T>(),
                new CCRParallelChains<T>(),
                new CCRTotalAsync<T>(),
                new PrimeOneChain<T>(),
                new PrimeParallelChains<T>(),
                new PrimeAsync<T>()
            };

            foreach (var e in Tests)
            {
                e.Run(Length, Width, Waves, Comp);
            }
        }


    }
}
