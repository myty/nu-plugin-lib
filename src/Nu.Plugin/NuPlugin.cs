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
                    OkResponse(_signature);
                }
                else if (req.Method == "sink")
                {
                    var requestParams = req.GetParams<IEnumerable<JsonRpcParams>>();
                    plugin.Sink(requestParams);
                }

                return true;
            });
        }

        public async Task FilterPluginAsync<TFilterPlugin>() where TFilterPlugin : INuPluginFilter, new()
        {
            await CommandHandler<TFilterPlugin>((req, plugin) =>
            {
                if (req.Method == "config")
                {
                    return OkResponse(_signature);
                }

                if (req.Method == "begin_filter")
                {
                    OkResponse(plugin.BeginFilter());
                }
                else if (req.Method == "filter")
                {
                    var requestParams = req.GetParams<JsonRpcParams>();

                    RpcValueResponse(plugin.Filter(requestParams));
                }
                else if (req.Method == "end_filter")
                {
                    return OkResponse(plugin.EndFilter());
                }

                return false;
            });
        }

        private async Task CommandHandler<TPluginType>(Func<JsonRpcRequest, TPluginType, bool> done) where TPluginType : new()
        {
            var plugin = new TPluginType();

            using (var standardInput = new StreamReader(_stdin, Console.InputEncoding))
            using (_standardOutputWriter = new StreamWriter(_stdout, Console.OutputEncoding))
            {
                _standardOutputWriter.AutoFlush = true;

                while (true)
                {
                    var request = await standardInput.GetNextRequestAsync();

                    if (request is null
                        || !request.IsValid
                        || done(request, plugin)) { break; }
                }
            }
        }

        private bool OkResponse(object response) => Response(new JsonRpcOkResponse(response));

        private bool RpcValueResponse(JsonRpcParams rpcParams) => Response(new JsonRpcValueResponse(rpcParams));

        private bool Response(JsonRpcResponse response)
        {
            var serializedResponse = JsonSerializer.Serialize(response);

            _standardOutputWriter.WriteLine(serializedResponse);

            return true;
        }
    }
}
