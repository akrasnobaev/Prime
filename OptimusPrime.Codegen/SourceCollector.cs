using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimusPrime.Codegen
{




    static class SourceCollectorCodegen
    {
        static string GenerateBasic(int N, Func<int, string> element, int from, string delimiter)
        {
            string result = "";
            for (int i = from; i < N; i++)
            {
                if (i != 0) result += delimiter;
                result += element(i);
            }
            return result;
        }

        static string Generate(int N, Func<int, string> element, string delimiter = "\n")
        {
            return GenerateBasic(N, element, 0, delimiter);
        }

        static string GenerateFrom1(int N, Func<int, string> element, string delimiter = "\n")
        {
            return GenerateBasic(N, element, 1, delimiter);
        }

        static string GenerateByTemplate(int N, string template, string delimiter = "\n")
        {
            return GenerateBasic(N, z => template.Replace("#", z.ToString()), 0, delimiter);
        }


        static string GenericParam(int N) { return "<" + GenerateByTemplate(N, "T#", ",") + ">"; }

        const int Max = 6;

        static string FactoryExtensions()
        {

            return GenerateFrom1(Max,
                z => string.Format(@"
public static SourceCollectorHelper{0} BindSources{0}(this IFactory factory, {1})
{{       
        return new SourceCollectorHelper{0}
        {{
            readers = new ISourceReader[]
            {{
                {2}
            }}
        }};
}}",
                    GenericParam(z),
                    GenerateByTemplate(z, "ISource<T#> source#", ","),
                    GenerateByTemplate(z, "source#.CreateReader()", ",")
                    ));

        }

        static string SourceCollectorHelper()
        {
            return GenerateFrom1(Max, z =>
                string.Format(@"
 public class SourceCollectorHelper{0} : SourceCollectorHelper
    {{
        public SourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection{0}, new()
        {{
            return new SourceCollector<TOutput>(readers);
        }}
    }}",
                GenericParam(z)));


        }

        static string SourceDataCollection()
        {
            return GenerateFrom1(Max, z => string.Format(@"
    public class SourceDataCollection{0} : ISourceDataCollection
    {{
        {1}        

        public int ListCount
        {{
            get {{ return {2}; }}
        }}
        public void Pull(int index, System.Collections.IEnumerable source)
        {{
            {3}
        }}
    }}",
                    GenericParam(z),
                    GenerateByTemplate(z, "protected List<T#> List#;"),
                    z,
                    GenerateByTemplate(z, "if (index==#) { if (List#==null) List#=new List<T#>(); List#.AddRange(source.Cast<T#>()); }")
                    ));
        }


        [STAThread]
        public static void Main()
        {
            var result = string.Format(@"
using OptimusPrime.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimusPrime.Factory
{{
  
    partial class IFactoryExtensions
    {{
        {0}
    }}

    {1}

    {2}
}}",
            FactoryExtensions(),
            SourceCollectorHelper(),
            SourceDataCollection());

            Console.WriteLine(result);
            Clipboard.SetText(result);


        
        }

    }
}

            
