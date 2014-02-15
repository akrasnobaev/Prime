using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler
{
  


    class Program
    {




        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, s) => { Console.Write("ER"); };
            Console.WriteLine(new HardComputations().MeasureTime());

            var sets = new TestSet[]
            {
                new TestSet<SmallData>(10,10,1000,new EasyComputations()),
                new TestSet<MediumData>(10,10,1000,new EasyComputations()),
                new TestSet<SmallData>(10,10,1000,new MediumComputations()),
                new TestSet<MediumData>(10,10,1000,new MediumComputations()),
                new TestSet<SmallData>(10,10,1000,new HardComputations()),
                new TestSet<MediumData>(10,10,1000,new HardComputations()),
               // new TestSet<BigData>(10,10,1000,new EasyComputations())
            };

            TestSet.RunAll(sets);
            Console.WriteLine(TestSet.PrintAll(sets));
          

            Console.ReadLine();
        }
    }
}
