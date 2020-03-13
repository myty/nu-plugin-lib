using System.Collections.Generic;

namespace Nu.Plugin.Interfaces
{
    public interface INuPluginSink
    {
        void Sink(IEnumerable<JsonRpcValue> requestParams);
    }
}
