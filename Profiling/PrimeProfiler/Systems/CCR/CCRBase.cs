using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ccr.Core;

namespace PrimeProfiler
{
    abstract class  CCRBase<T> : TestSystem
        where T : Data,new()
    {
        protected Port<T>[] InputPorts;
      
        Dispatcher dr;
        DispatcherQueue taskQueue;
       
        protected int[] flags;

        protected override void Initialize()
        {
            dr = new Dispatcher();
            taskQueue = new DispatcherQueue("samples", dr);

            var ports = new Port<T>[Length, Width];
            flags = new int[Width];

            for (int l=0;l<Length;l++)
                for (int c=0;c<Width;c++)
                    ports[l, c] = new Port<T>();

            for (int l = 0; l < Length - 1; l++)
                for (int c = 0; c < Width; c++)
                {

                    var ll = l;
                    var cc = c;
                    Arbiter.Activate(taskQueue, Arbiter.Receive(true, ports[ll, cc], s => { s.Marker=Computations.Compute(s.Marker); ports[ll + 1, cc].Post(s); }));
                }
            for (int c = 0; c < Width; c++)
            {
                int cc = c;
                Arbiter.Activate(taskQueue, Arbiter.Receive(true, ports[Length - 1, c], s => { s.Marker = Computations.Compute(s.Marker); lock (flags) { flags[cc]++; }  }));
            }
            InputPorts = new Port<T>[Width];
            for (int i = 0; i < Width; i++)
                InputPorts[i] = ports[0, i];
        }

        protected override void Finish()
        {
            taskQueue.Dispose();
            dr.Dispose();
        }

        public override Type GetDataType
        {
            get { return typeof(T);  }
        }
    }
}
