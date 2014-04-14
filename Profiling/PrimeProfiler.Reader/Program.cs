using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeProfiler.Reader
{
    class Program
    {
        static Dictionary<string, string> names = new Dictionary<string, string>();
        static Dictionary<string, bool> sync = new Dictionary<string, bool>();
        static Dictionary<string, int> number = new Dictionary<string, int>();
        static int currentNumber = 0;
        static void Add(Type type, string name, bool issync=true)
        {
            names[type.Name] = name;
            sync[type.Name] = issync;
            number[name] = currentNumber;
            currentNumber++;
        }


        [STAThread]
        public static void Main()
        {
            Add(typeof(For<>), "FOR");
            Add(typeof(LinqParallelChains<>), "Radiant Chains");
            Add(typeof(PrimeParallelChains<>), "Liberty Chains");
            Add(typeof(CCRParallelChains<>), "CCR (in sync mode)");
            Add(typeof(PrimeAsync<>), "Prime Sources", false);
            Add(typeof(CCRTotalAsync<>), "CCR (in async mode)", false);



            var entries = File
                .ReadAllLines("..\\..\\..\\result.txt")
                .Select(z => z.Split('\t'))
                .Select(z => new { Async = z[1] != "1", Data = z[4], System = z[3], Time = double.Parse(z[6]) })
                .ToArray();

            var results = entries
                .GroupBy(z => new { Async=z.Async, Data = z.Data, System = z.System })
                .Select(z =>
                    {
                        var average = z.Average(x=>x.Time);
                         var deviation = z.Select(x => Math.Pow(x.Time - average, 2)).Sum();
                        deviation = Math.Sqrt(deviation / z.Count());
                        return new
                        {
                        System = names[z.Key.System],
                        IsSystemSync = sync[z.Key.System],
                        Data = z.Key.Data,
                        IsTestSync = !z.Key.Async,
                        Average = average,
                        Deviation = deviation,
                        };
                    })
                .ToArray();

            StringBuilder bld = new StringBuilder();


            foreach(var table in results.GroupBy(z=>z.IsTestSync))
            {
                bld.Append(table.Key?"Synchronous mode" : "Asynchronous mode");
                bld.Append("\n\n");
                bld.Append(@"
$$
\begin{array}{| l | c | c | c |}
\hline
               & $Small data$ & $Medium data$ & $Big data$ \\
\hline 
");
                foreach(var subtable in table.GroupBy(z=>z.IsSystemSync))
                {
                    foreach (var row in subtable.GroupBy(z => z.System).OrderBy(z => number[z.Key]))
                    {
                        bld.Append("$"+row.Key+"$\t");
                        foreach(var result in row)
                        {
                            bld.Append(string.Format("& {0} \\pm {1} \t",Math.Round(result.Average,3),Math.Round(result.Deviation,3)));
                        }
                        bld.Append("\\\\\n");
                    }
                    bld.Append("\n\\hline\n");
                }

                bld.Append(@"
\end{array}
$$

");
            }

            Console.WriteLine(bld.ToString());

            Clipboard.SetText(bld.ToString());
        }
    }
}
