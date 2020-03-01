using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin
{
    public partial class NuPlugin
    {
        internal class PluginResponse<TPluginType> where TPluginType : new()
        {
            readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();
            readonly StreamWriter _writer;
            readonly TPluginType _plugin;

            public PluginResponse(StreamWriter writer, TPluginType plugin)
            {
                _plugin = plugin;
                _writer = writer;
            }

            public void Config(Signature signature)
            {
                var rpcResponse = JsonRpcResponse.Ok(signature);

                Respond(rpcResponse);
                Done();
            }

            public void Sink(IEnumerable<JsonRpcParams> requestParams)
            {
                if (_plugin is INuPluginSink sinkPlugin)
                {
                    sinkPlugin.Sink(requestParams);
                }

                Done();
            }

            public void BeginFilter()
            {
                if (_plugin is INuPluginFilter filterPlugin)
                {
                    var rpcResponse = JsonRpcResponse.Ok(
                        filterPlugin.BeginFilter()
                    );

                    Respond(rpcResponse);
                }

                Continue();
            }

            public void Filter(JsonRpcParams requestParams)
            {
                if (_plugin is INuPluginFilter filterPlugin)
                {
                    Respond(
                        JsonRpcResponse.RpcValue(
                            filterPlugin.Filter(requestParams)
                        )
                    );
                }

                Continue();
            }

            public void EndFilter()
            {
                if (_plugin is INuPluginFilter filterPlugin)
                {
                    var rpcResponse = JsonRpcResponse.Ok(
                        filterPlugin.EndFilter()
                    );

                    Respond(rpcResponse);
                }

                Continue();
            }

            public void Quit() => Done();

            private void Respond(JsonRpcResponse rpcResponse) =>
                _writer.WriteLine(JsonSerializer.Serialize(rpcResponse));

            private void Continue() => _tcs.SetResult(true);

            private void Done() => _tcs.SetResult(false);

            public Task<bool> RespondAsync() => _tcs.Task;
        }
    }
}
