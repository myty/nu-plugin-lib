using System.Linq;
using System;
using System.Collections.Generic;
using Nu.Plugin.Interfaces;
using Nu.Plugin.JsonRpc;

namespace Nu.Plugin.Len
{
    public class LengthFilter : INuPluginFilter
    {
        public Result<IEnumerable<Result<IReturnSuccess>>> BeginFilter()
        {
            return new OkResult<IEnumerable<Result<IReturnSuccess>>>(Enumerable.Empty<Result<IReturnSuccess>>());
        }

        public Result<IEnumerable<Result<IReturnSuccess>>> Filter(JsonRpcValue requestParams)
        {
            var stringLength = requestParams.Value.Primitive["String"].ToString().Length;

            requestParams.Value.Primitive = new Dictionary<string, object>
            {
                {"Int", stringLength}
            };

            return new OkResult<IEnumerable<Result<IReturnSuccess>>>(
                new OkResult<IReturnSuccess>[]
                {
                    new OkResult<IReturnSuccess>(
                        new ValueReturnSuccess(requestParams)
                    )
                }
            );
        }

        public Result<IEnumerable<Result<IReturnSuccess>>> EndFilter()
        {
            return new OkResult<IEnumerable<Result<IReturnSuccess>>>(Enumerable.Empty<Result<IReturnSuccess>>());
        }
    }
}
