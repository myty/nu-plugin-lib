using System;
using System.Threading.Tasks;

namespace Nu.Plugin
{
    public interface INuPluginBuilder
    {
        INuPluginBuilder Name(string name);

        INuPluginBuilder Usage(string usage);

        INuPluginBuilder Filter(
            Func<object> beginFilter,
            Func<JsonRpcParams, JsonRpcParams> filter,
            Func<object> endFilter);

        Task RunAsync();
    }
}
