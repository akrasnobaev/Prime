using System;
using System.Windows.Forms;

namespace Prime
{




    class SourceCollectorCodegen : GenerationBase
    {
       
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
        [Obsolete(""Используйте CreateSyncCollector или CreateAsyncCollector. Этот метод производит тот же коллектор, что и раньше. Он теперь называется асинхронным"")]
        public AsyncSourceCollector<TOutput> CreateCollector<TOutput>()
            where TOutput : SourceDataCollection{0}, new()
        {{
            return new AsyncSourceCollector<TOutput>(readers);
        }}

        public AsyncSourceCollector<TOutput> CreateAsyncCollector<TOutput>()
            where TOutput : SourceDataCollection{0}, new()
        {{
            return new AsyncSourceCollector<TOutput>(readers);
        }}

        public SyncSourceCollector<TOutput> CreateSyncCollector<TOutput>()
            where TOutput : SyncronousSourceDataCollection{0}, new()
        {{
            return new SyncSourceCollector<TOutput>(readers);
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

        static string SyncSourceDataCollection()
        {
            return GenerateFrom1(Max, z => string.Format(@"
    public class SyncronousSourceDataCollection{0} : ISyncronousDataCollection
    {{
        public int FieldsCount
        {{
            get {{ return {1}; }}
        }}
        public void GetOne(int index, object data)
        {{
            {2}
        }}

        {3}
    }}",
                    GenericParam(z),
                    z,
                    GenerateByTemplate(z, "if (index==#) { Data#=(T#)data; }"),
                     GenerateByTemplate(z, "public T# Data#;")
                 
                    ));
        }


        [STAThread]
        public static void Main1()
        {
            var result = string.Format(@"
using Prime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime
{{
  
    partial class IFactoryExtensions
    {{
        {0}
    }}

    {1}

    {2}

    {3}
}}",
            FactoryExtensions(),
            SourceCollectorHelper(),
            SourceDataCollection(),
            SyncSourceDataCollection());

            Console.WriteLine(result);
            Clipboard.SetText(result);


        
        }

    }
}

            
