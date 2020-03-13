using System.Collections.Generic;
using Nu.Plugin.JsonRpc;

namespace Nu.Plugin.Interfaces
{
    public interface INuPluginFilter
    {
        Result<IEnumerable<Result<IReturnSuccess>>> BeginFilter();
        Result<IEnumerable<Result<IReturnSuccess>>> Filter(JsonRpcValue requestParams);
        Result<IEnumerable<Result<IReturnSuccess>>> EndFilter();
    }
}
