using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin
{
    public class NuPlugin
    {
        private readonly Stream _stdin;
        private readonly Stream _stdout;

        private Signature _signature = Signature.Create();
        private StreamWriter _standardOutputWriter;

        private NuPlugin(Stream stdin, Stream stdout)
        {
            _stdout = stdout;
            _stdin = stdin;
        }

        public static NuPlugin Build(string name = null)
        {
            var stdin = Console.OpenStandardInput();
            var stdout = Console.OpenStandardOutput();

            return new NuPlugin(stdin, stdout).Name(name);
        }

        public NuPlugin Name(string name)
        {
            _signature = _signature.WithName(name);
            return this;
        }

        public NuPlugin Description(string description)
        {
            _signature = _signature.WithDescription(description);
            return this;
        }

        public async Task SinkPluginAsync<TSinkPlugin>() where TSinkPlugin : INuPluginSink, new()
        {
            await CommandHandler<TSinkPlugin>((req, plugin) =>
            {
                if (req.Method == "config")
                {
                    Done(_signature);
                }
                else if (req.Method == "sink")
                {
                    var requestParams = req.GetParams<IEnumerable<JsonRpcParams>>();
                    plugin.Sink(requestParams);
                }

                return Done();
            });
        }

        public async Task FilterPluginAsync<TFilterPlugin>() where TFilterPlugin : INuPluginFilter, new()
        {
            await CommandHandler<TFilterPlugin>((req, plugin) =>
            {
                if (req.Method == "config")
                {
                    return Done(_signature.WithIsFilter(true));
                }
                else if (req.Method == "begin_filter")
                {
                    return Continue(plugin.BeginFilter());
                }
                else if (req.Method == "filter")
                {
                    var requestParams = req.GetParams<JsonRpcParams>();

                    return RpcValueResponse(plugin.Filter(requestParams));
                }
                else if (req.Method == "end_filter")
                {
                    return Done(plugin.EndFilter());
                }

                return Done();
            });
        }

        private async Task CommandHandler<TPluginType>(Func<JsonRpcRequest, TPluginType, PluginResponse> response) where TPluginType : new()
        {
            var plugin = new TPluginType();

            using (var standardInput = new StreamReader(_stdin, Console.InputEncoding))
            using (_standardOutputWriter = new StreamWriter(_stdout, Console.OutputEncoding))
            {
                _standardOutputWriter.AutoFlush = true;

                while (true)
                {
                    var request = await standardInput.GetNextRequestAsync();

                    if (request?.IsValid != true) { break; }

                    switch (response(request, plugin))
                    {
                        case DoneResponse done:
                            break;
                    }
                }
            }
        }

        private PluginResponse Done(object response = null) => response == null
            ? new DoneResponse()
            : Response(new JsonRpcOkResponse(response), true);

        private PluginResponse Continue(object response) => Response(new JsonRpcOkResponse(response));

        private PluginResponse RpcValueResponse(JsonRpcParams rpcParams) => Response(new JsonRpcValueResponse(rpcParams));

        private PluginResponse Response(JsonRpcResponse response, bool isDone = false)
        {
            var serializedResponse = JsonSerializer.Serialize(response);

            _standardOutputWriter.WriteLine(serializedResponse);

            if (isDone)
            {
                return new DoneResponse();
            }

            return new ContinueResponse();
        }

        internal abstract class PluginResponse { }

        internal class ContinueResponse : PluginResponse { }
        internal class DoneResponse : PluginResponse { }
    }
}
