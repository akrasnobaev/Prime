using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptimusPrime.Codegen
{
    class GenerationBase
    {
       public static string GenerateBasic(int N, Func<int, string> element, int from, string delimiter)
        {
            string result = "";
            for (int i = from; i < N; i++)
            {
                if (i != 0) result += delimiter;
                result += element(i);
            }
            return result;
        }

       public static string Generate(int N, Func<int, string> element, string delimiter = "\n")
        {
            return GenerateBasic(N, element, 0, delimiter);
        }

       public static string GenerateFrom1(int N, Func<int, string> element, string delimiter = "\n")
        {
            return GenerateBasic(N, element, 1, delimiter);
        }

       public static string GenerateByTemplate(int N, string template, string delimiter = "\n")
        {
            return GenerateBasic(N, z => template.Replace("#", z.ToString()), 0, delimiter);
        }


       public static string GenericParam(int N) { return "<" + GenerateByTemplate(N, "T#", ",") + ">"; }

       public const int Max = 6;

    }
}
