using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nu.Plugin;

namespace Myty.Nu.Plugin.Len
{
    class Program
    {
        static JsonRpcParams Filter(JsonRpcParams requestParams)
        {
            var stringLength = requestParams.Value.Primitive["String"].ToString().Length;

            requestParams.Value.Primitive = new Dictionary<string, object>{
                {"Int", stringLength}
            };

            return requestParams;
        }

        static async Task Main(string[] args)
        {
            var stdin = Console.OpenStandardInput();
            var stdout = Console.OpenStandardOutput();

            await NuPlugin.Create(stdin, stdout)
                .Name("len")
                .Usage("Return the length of a string")
                .Filter(
                    beginFilter: () => Array.Empty<string>(),
                    filter: Program.Filter,
                    endFilter: () => Array.Empty<string>()
                )
                .RunAsync();
        }
    }
}
