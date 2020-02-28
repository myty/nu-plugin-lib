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

        public async Task SinkPluginAsync<T>() where T : INuPluginSink, new()
        {
            _signature = _signature.WithIsFilter(true);
            var sink = new T();

            using (var standardInput = new StreamReader(_stdin, Console.InputEncoding))
            using (_standardOutputWriter = new StreamWriter(_stdout, Console.OutputEncoding))
            {
                _standardOutputWriter.AutoFlush = true;

                while (true)
                {
                    var request = await standardInput.GetNextRequestAsync();

                    if (request is null || !request.IsValid) { break; }

                    if (request.Method == "config")
                    {
                        OkResponse(_signature);
                        break;
                    }
                    else if (request.Method == "sink")
                    {
                        var requestParams = request.GetParams<IEnumerable<JsonRpcParams>>();
                        sink.Sink(requestParams);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public async Task FilterPluginAsync<T>() where T : INuPluginFilter, new()
        {
            _signature = _signature.WithIsFilter(true);
            var filter = new T();

            using (var standardInput = new StreamReader(_stdin, Console.InputEncoding))
            using (_standardOutputWriter = new StreamWriter(_stdout, Console.OutputEncoding))
            {
                _standardOutputWriter.AutoFlush = true;

                while (true)
                {
                    var request = await standardInput.GetNextRequestAsync();

                    if (request is null || !request.IsValid) { break; }

                    if (request.Method == "config")
                    {
                        OkResponse(_signature);
                        break;
                    }
                    else if (_signature.IsFilter && request.Method == "begin_filter")
                    {
                        OkResponse(filter.BeginFilter());
                    }
                    else if (request.Method == "filter")
                    {
                        var requestParams = request.GetParams<JsonRpcParams>();

                        RpcValueResponse(filter.Filter(requestParams));
                    }
                    else if (request.Method == "end_filter")
                    {
                        OkResponse(filter.EndFilter());
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void OkResponse(object response) => Response(new JsonRpcOkResponse(response));

        private void RpcValueResponse(JsonRpcParams rpcParams) => Response(new JsonRpcValueResponse(rpcParams));

        private void Response(JsonRpcResponse response)
        {
            var serializedResponse = JsonSerializer.Serialize(response);

            _standardOutputWriter.WriteLine(serializedResponse);
        }
    }
}
