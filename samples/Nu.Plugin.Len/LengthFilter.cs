using System;
using System.Collections.Generic;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin.Len
{
    public class LengthFilter : INuPluginFilter
    {
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
