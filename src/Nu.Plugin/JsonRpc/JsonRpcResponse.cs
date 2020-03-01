using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    internal class JsonRpcResponse
    {
        public JsonRpcResponse(object rpcResponseParams) => Params = rpcResponseParams;

        [JsonPropertyName("jsonrpc")]
        public string JsonRPC { get; } = "2.0";

        [JsonPropertyName("method")]
        public string Method { get; } = "response";

        [JsonPropertyName("params")]
        public object Params { get; }

        public static JsonRpcResponse Ok(object okResponseParams) =>
            new JsonRpcResponse(new {
                Ok = okResponseParams
            });

        public static JsonRpcResponse RpcValue(object rpcParams) =>
            Ok(new object[] {
                new {
                    Ok = new {
                        Value = rpcParams
                    }
                }
            });
    }
}