using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler.Reader
{
    class Program
    {
        public static void Main()
        {
            var entries = File
                .ReadAllLines("..\\..\\..\\result.txt")
                .Select(z => z.Split('\t'))
                .Select(z => new { Async = z[1] != "1", Data = z[4], System = z[3], Time = double.Parse(z[6]) })
                .ToArray();

            foreach (var table in entries.GroupBy(z => z.Async))
            {
                Console.WriteLine(table.Key ? "Async" : "Sync");
                foreach (var system in table.GroupBy(z => z.System))
                {
                    Console.Write("{0,-25}", system.Key);
                    foreach (var data in system.GroupBy(z => z.Data))
                    {
                        Console.Write(data.Key[0]+" ");
                        var average = data.Average(z => z.Time);
                        Console.Write("{0,-10}", Math.Round(average, 3));
                    }
                    Console.WriteLine();
                }
            }

            
        }
    }
}
