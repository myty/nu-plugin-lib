using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Myty.Nu.Plugin.Len
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (var standardInput = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
                {
                    while (true)
                    {
                        var request = await standardInput.GetNextRequestAsync();

                        if (request is null || !request.IsValid) { break; }

                        if (request.Method == "config")
                        {
                            PluginConfiguration
                                .CreateResponse(
                                    name: "len",
                                    usage: "Return the length of a string"
                                )
                                .Respond();
                            break;
                        }
                        else if (request.Method == "begin_filter")
                        {
                            new JsonRpcOkResponse(Array.Empty<string>()).Respond();
                        }
                        else if (request.Method == "filter")
                        {
                            var requestParams = request.GetParams<JsonRpcParams>();

                            var stringLength = requestParams.Value.Primitive["String"].ToString().Length;
                            requestParams.Value.Primitive = new Dictionary<string, object>{
                                {"Int", stringLength.ToString()}
                            };

                            new JsonRpcValueResponse(requestParams).Respond();
                        }
                        else if (request.Method == "end_filter")
                        {
                            new JsonRpcOkResponse(Array.Empty<string>()).Respond();
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.Log("Exception", ex.ToString());
            }
        }
    }

    internal static class StreamReaderExtension
    {
        internal static async Task<JsonRpcRequest> GetNextRequestAsync(this StreamReader reader)
        {
            var json = await reader.ReadLineAsync();
            Logger.Log("Request", json);

            if (!string.IsNullOrEmpty(json?.Trim()))
            {
                return new JsonRpcRequest(json);
            }

            return null;
        }
    }

    internal static class Logger
    {
        internal static void Log(string type, string json)
        {
            var now = DateTime.Now;

            try
            {
                _ = File.AppendAllTextAsync(
                    $@"D:\dev\mytydev\nu\nu_plugins\dotnet\nu_plugin_len\log-{now:yyyy-MM-dd}.txt",
                    $"[{now:R}] - {type.PadRight(9)} - {json}{Environment.NewLine}"
                );
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    internal class PluginConfiguration
    {
        private PluginConfiguration(string name, string usage)
        {
            Name = name;
            Usage = usage;
        }

        public static JsonRpcResponse CreateResponse(string name, string usage)
        {
            var configuration = new PluginConfiguration(name, usage);

            return new JsonRpcOkResponse(configuration);
        }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("usage")]
        public string Usage { get; }

        [JsonPropertyName("positional")]
        public int[] Positional { get; set; } = Array.Empty<int>();

        [JsonPropertyName("named")]
        public object Named { get; set; } = new { };

        [JsonPropertyName("is_filter")]
        public bool IsFilter { get; set; } = true;
    }

    internal class JsonRpcRequest
    {
        readonly JsonDocument _jsonDoc;
        readonly bool _isValid = true;

        public JsonRpcRequest(string json)
        {
            if (string.IsNullOrEmpty(json?.Trim()))
            {
                _isValid = false;
            }
            else
            {
                _jsonDoc = JsonDocument.Parse(json);

                var rootElement = _jsonDoc.RootElement;

                if (!rootElement.TryGetProperty("jsonrpc", out var jsonRpcValue)
                    || jsonRpcValue.GetString() != "2.0")
                {
                    _isValid = false;
                }

                if (!rootElement.TryGetProperty("method", out var methodValue))
                {
                    _isValid = false;
                }

                Method = methodValue.GetString();
            }
        }

        public string Method { get; }

        public T GetParams<T>()
        {
            return JsonSerializer.Deserialize<T>(_jsonDoc.RootElement.GetProperty("params").GetRawText());
        }

        public bool IsValid => _isValid;
    }

    internal abstract class JsonRpcResponse
    {
        [JsonPropertyName("jsonrpc")]
        public string JsonRPC { get; } = "2.0";

        [JsonPropertyName("method")]
        public string Method { get; } = "response";

        [JsonPropertyName("params")]
        public abstract object Params { get; }

        public void Respond()
        {
            var serializedResponse = JsonSerializer.Serialize(this);

            Logger.Log("Response", serializedResponse);

            Console.WriteLine(serializedResponse);
        }
    }

    internal class JsonRpcParams
    {
        [JsonPropertyName("value")]
        public ParamValue Value { get; set; }

        [JsonPropertyName("tag")]
        public ParamTag Tag { get; set; }

        public class ParamValue
        {
            public IDictionary<string, object> Primitive { get; set; }
        }

        public class ParamTag
        {
            [JsonPropertyName("anchor")]
            public string Anchor { get; set; }

            [JsonPropertyName("span")]
            public ParamTagSpan Span { get; set; }

            public class ParamTagSpan
            {
                [JsonPropertyName("start")]
                public int Start { get; set; }

                [JsonPropertyName("end")]
                public int End { get; set; }
            }
        }
    }

    internal class JsonRpcOkResponse : JsonRpcResponse
    {
        public JsonRpcOkResponse(object okResponseParams) => Params = new
        {
            Ok = okResponseParams
        };

        public override object Params { get; }
    }

    internal class JsonRpcValueResponse : JsonRpcResponse
    {
        public JsonRpcValueResponse(JsonRpcParams rpcParams) => Params = rpcParams;

        public override object Params { get; }
    }
}
