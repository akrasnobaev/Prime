using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler
{
  


    class Program
    {

        static void SingleTest()
        {
            TestSystem system=null;

            //system = new PrimeParallelChains<SmallData>();
            //system = new LinqOneChain<SmallData>();
            //system = new For<SmallData>();

            var comp = new EasyComputations();
            comp.MeasureTime();
            system.Run(100, 1, 100000, comp);
            
            var total=system.ElapsedMS;
            var overheads=total/(system.Length*system.WaveCount);
            
            Console.WriteLine("{0}\n{1}\n{2}\n{3} microS", total,comp.Time,overheads,1000*(overheads-comp.Time));
            Console.ReadKey();
        }


        static void TestMode()
        {
            SingleTest();
           // new EasyCompu1tations().MeasureTime();
        }

        static void BatchMode(string[] args)
        {
          

            var length = int.Parse(args[0]);
            var count = int.Parse(args[1]);
            var waves = int.Parse(args[2]);

            var systems = new[] { 
                typeof(For<>),
                typeof(CCRParallelChains<>), 
                typeof(CCRTotalAsync<>), 
                typeof(PrimeParallelChains<>), 
                typeof(LinqParallelChains<>),
                typeof(PrimeAsync<>)
            };

            var datas = new[] {
                typeof(SmallData),
                typeof(MediumData)
            };

            var comps = new Computations[] {
                new EasyComputations(),
                new MediumComputations()
            };


            Type system = systems[int.Parse(args[3])];
            Type data = datas[int.Parse(args[4])];
            var comp = comps[int.Parse(args[5])];

            Console.WriteLine("Waves {0}\nSystem {1}\nData {2}\nComp {3}",
                waves,
            system.Name,
            data.Name,
            comp.GetType().Name);

            var s = (TestSystem)system.MakeGenericType(new[] { data }).GetConstructor(new Type[] { }).Invoke(new object[] { });

            comp.MeasureTime();

            s.Run(length, count, waves, comp);

            var writer = new StreamWriter("result.txt", true);
            writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                length,
                count,
                waves,
                system.Name,
                data.Name,          
                s.ElapsedMS
               );
            writer.Close();
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0) TestMode();
            else BatchMode(args);
        }
    }
}
