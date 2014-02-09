using System;
using System.Windows.Forms;

namespace Prime
{
    class Union : GenerationBase
    {

        static string Method(int N)
        {
            return string.Format(@"
public static IChain<TIn, Tuple<{0}>> Union<TIn, {0}>(this IFactory factory, {1})
        {{
            {2}
            return factory.CreateChain<TIn, Tuple<{0}>>(
                z => Tuple.Create({3}));
        }}
"
                , GenerateByTemplate(N, "T#",",")
                , GenerateByTemplate(N, "IChain<TIn,T#> chain# ",",")
                , GenerateByTemplate(N, "var block#=chain#.ToFunctionalBlock();")
                , GenerateByTemplate(N, "block#.Process(z)",",")
                
                );
        }

        [STAThread]
        public static void Main()
        {
            string result = string.Format(@"
using Prime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime
{{
    public static partial class FactoryExtension
    {{
         {0}
    }}
}}", GenerateBasic(Max, z => Method(z), 2, "\n"));

            Console.WriteLine(result);
            Clipboard.SetText(result);



        }


    }
}
