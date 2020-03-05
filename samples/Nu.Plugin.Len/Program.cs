using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin.Len
{
    internal class Program : INuPluginFilter
    {
        private static async Task Main() => await NuPlugin
                                                  .Build("len")
                                                  .Description("Return the length of a string")
                                                  .FilterPluginAsync<Program>();

        public object BeginFilter() => Array.Empty<string>();

        public JsonRpcParams Filter(JsonRpcParams requestParams)
        {
            var stringLength = requestParams.Value.Primitive["String"].ToString().Length;

            requestParams.Value.Primitive = new Dictionary<string, object>
            {
                {"Int", stringLength}
            };

            return requestParams;
        }

        public object EndFilter() => Array.Empty<string>();
    }
}
