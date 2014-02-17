using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler
{

    public class HardComputations : Computations
    {
        public override double Compute(double input)
        {
            return UselessFunction(input, 1000);
        }
    }

    public class MediumComputations : Computations
    {
        public override double Compute(double input)
        {
            return UselessFunction(input, 40000);
        }
    }
        
    public class EasyComputations : Computations
    {
        public override double Compute(double input)
        {
            return UselessFunction(input, 10);
        }
    }

    public abstract class Computations
    {
        public abstract double Compute(double input);

        public double Time { get; private set; }

        public void MeasureTime(int count = -1)
        {

            if (count == -1)
            {
                count = 1;

                var watch1 = new Stopwatch();
                watch1.Start();
                while (watch1.ElapsedMilliseconds < 100)
                {
                    Compute(1);
                    count++;
                }
                watch1.Stop();
                count *= 2;
            }
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < count; i++) Compute(1);
            watch.Stop();
            Time = (double)watch.ElapsedMilliseconds / count;
            Console.WriteLine("Measure " + GetType().Name + " : " + Time);
        }



        protected double UselessFunction(double input, int iterationNumber)
        {
            for (int i = 0; i < iterationNumber; i++)
                input /= 1.02;
            input += Rnd.Double();
            return input;
        }
    }
}
