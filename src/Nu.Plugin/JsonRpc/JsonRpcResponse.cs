using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Nu.Plugin.JsonRpc;

namespace Nu.Plugin
{
    internal class JsonRpcResponse
    {
        public JsonRpcResponse(object rpcResponseParams) => Params = rpcResponseParams;

        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; } = "2.0";

        [JsonPropertyName("method")]
        public string Method { get; } = "response";

        [JsonPropertyName("params")]
        public object Params { get; }

        public static JsonRpcResponse Ok(Result result) => new JsonRpcResponse(result);
        public static JsonRpcResponse Ok(object okResponseParams) => Ok(new OkResult(okResponseParams));
        public static JsonRpcResponse Ok(IEnumerable<JsonRpcValue> rpcValues) => Ok(rpcValues.Select(v => new OkResult<JsonRpcValue>(v)));
        public static JsonRpcResponse Ok(JsonRpcValue rpcValue) => Ok(new JsonRpcValue[] { rpcValue });
    }
}
