using System;

namespace OptimusPrime.OprimusPrimeCore.Helpers
{
    public static class ServiceNameHelper
    {
        public static string GetInName()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetOutName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}