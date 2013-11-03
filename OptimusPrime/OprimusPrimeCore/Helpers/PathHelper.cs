using System;
using System.IO;

namespace OptimusPrime.OprimusPrimeCore.Helpers
{
    public class PathHelper
    {
        private const string OptimusPrimeHomeDirectory = "OptimusPrimeLogs";

        public static string GetFilePath()
        {
            return String.Format("{0}\\{1:yyyy.MM.dd_hh.mm.ss.fff}.opdump", GetFolderPath(), DateTime.Now);
        }

        public static string GetFolderPath()
        {
            var folder = String.Format("{0}\\{1}\\{2}",
                                       Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                       OptimusPrimeHomeDirectory,
                                       DateTime.Now.ToShortDateString());
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }
    }
}