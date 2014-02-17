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



        static void TestMode()
        {
            new EasyComputations().MeasureTime();
        }

        static void BatchMode(string[] args)
        {
          

            var length = int.Parse(args[0]);
            var count = int.Parse(args[1]);
            var waves = int.Parse(args[2]);

            var systems = new[] { 
                typeof(CCRParallelChains<>), 
                typeof(CCRTotalAsync<>), 
                typeof(PrimeParallelChains<>), 
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
            writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
                length,
                count,
                waves,
                system.Name,
                data.Name,
                comp.GetType().Name,
                s.ElapsedMS,
                comp.Time);
            writer.Close();
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0) TestMode();
            else BatchMode(args);
        }
    }
}
