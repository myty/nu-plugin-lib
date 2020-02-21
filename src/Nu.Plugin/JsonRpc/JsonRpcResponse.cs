using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    internal abstract class JsonRpcResponse
    {
        [JsonPropertyName("jsonrpc")]
        public string JsonRPC { get; } = "2.0";

        [JsonPropertyName("method")]
        public string Method { get; } = "response";

        [JsonPropertyName("params")]
        public abstract object Params { get; }
    }
}