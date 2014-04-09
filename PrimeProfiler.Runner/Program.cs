using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeProfiler.Runner
{
    class Program
    {
        static void Run(int length, int widht, int count, int systemType, int dataType, int compType)
        {
            var process = new Process();
            process.StartInfo.FileName = "PrimeProfiler";
            process.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4} {5}", length, widht, count, systemType, dataType, compType);
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }

        static void MakeComp(int count, int comp)
        {
            for (int stype = 0; stype < 4; stype++)
                for (int dtype = 0; dtype < 2; dtype++)
                {
                    Run(10, 10, count, stype, dtype, comp);
                }
        }

        static void Main(string[] args)
        {
            File.Delete("result.txt");
            MakeComp(10000, 0);
            MakeComp(1000, 1);

        }
    }
}
