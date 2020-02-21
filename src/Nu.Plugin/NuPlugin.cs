using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin
{
    public class NuPlugin : INuPluginBuilder
    {
        private readonly Stream _stdin;
        private readonly Stream _stdout;

        private StreamWriter _standardOutputWriter;

        private PluginConfiguration _configuration = PluginConfiguration.Create();

        private INuPluginFilter _filter = null;

        private NuPlugin(Stream stdin, Stream stdout)
        {
            _stdout = stdout;
            _stdin = stdin;
        }

        public async Task RunAsync()
        {
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
                        OkResponse(_configuration);
                        break;
                    }
                    else if (_configuration.IsFilter && request.Method == "begin_filter")
                    {
                        OkResponse(_filter.BeginFilter());
                    }
                    else if (request.Method == "filter")
                    {
                        var requestParams = request.GetParams<JsonRpcParams>();

                        RpcValueResponse(_filter.Filter(requestParams));
                    }
                    else if (request.Method == "end_filter")
                    {
                        OkResponse(_filter.EndFilter());
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

        public static INuPluginBuilder Create(Stream stdin, Stream stdout) => new NuPlugin(stdin, stdout);

        public static INuPluginBuilder Create()
        {
            var stdin = Console.OpenStandardInput();
            var stdout = Console.OpenStandardOutput();

            return new NuPlugin(stdin, stdout);
        }

        public INuPluginBuilder Name(string name)
        {
            _configuration = _configuration.WithName(name);
            return this;
        }

        public INuPluginBuilder Usage(string usage)
        {
            _configuration = _configuration.WithUsage(usage);
            return this;
        }

        public INuPluginBuilder IsFilter<T>() where T : INuPluginFilter, new()
        {
            _configuration = _configuration.WithIsFilter(true);
            _filter = new T();

            return this;
        }
    }
}
