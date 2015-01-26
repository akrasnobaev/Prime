using System;
using System.IO;

namespace Prime
{
    public static partial class FactoryExtensions
    {

        public static void DumpDb(this IPrimeFactory factory, string filename, bool overwrite = true)
        {
            var f = factory.DumpDb();
            if (File.Exists(filename) && overwrite)
                File.Delete(filename);
            File.Move(f, filename);
        }

        public static EventBlock<T> CreateCustomLogger<T>(this IPrimeFactory factory, string loggingName)
        {
            var block = new EventBlock<T>();
            factory.CreateSource(block, loggingName);
            return block;
        }
    }
}