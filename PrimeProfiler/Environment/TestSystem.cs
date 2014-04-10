using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeProfiler
{
    abstract class TestSystem
    {
        public Computations Computations { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int WaveCount { get; set; }

        protected abstract void Initialize();
        protected abstract void RunWaves();
        protected abstract void Finish();

        public double ElapsedMS;

        public double TimePerService { get { return 8*ElapsedMS / (Length * Width * WaveCount); } }

        public void Run(int length, int widht, int waves, Computations comp)
        {
            Length = length;
            Width = widht;
            WaveCount = waves;
            Computations = comp;
            Run();
        }

        public void Run()
        {
            Console.Write("Init ");
            Initialize();
            var watch = new Stopwatch();
            Console.Write("Start "); 
            watch.Start();
            RunWaves();
            watch.Stop();
            Console.Write("Stop ");
            Thread.Sleep(100);
            try
            {
                Finish();
            }
            catch { }
            Console.WriteLine("OK");
            ElapsedMS= (double)watch.ElapsedMilliseconds;
            GC.Collect();
            GC.Collect();

        }

        public string GetName()
        {
            return this.GetType().GetGenericTypeDefinition().Name;
        }

        public abstract bool IsSync { get; }

        public abstract Type GetDataType { get; }
    }
}