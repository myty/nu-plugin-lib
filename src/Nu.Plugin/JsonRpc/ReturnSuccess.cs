using System.Text.Json.Serialization;

namespace Nu.Plugin.JsonRpc
{
    public interface IReturnSuccess { }

    public abstract class ReturnSuccessBase : IReturnSuccess
    {
        public ReturnSuccessBase(JsonRpcValue value) => Value = value;

        public JsonRpcValue Value { get; }
    }

    public class ValueReturnSuccess : ReturnSuccessBase
    {
        public ValueReturnSuccess(JsonRpcValue value) : base(value) { }

        [JsonPropertyName("Value")]
        public new JsonRpcValue Value => base.Value;
    }

    public class DebugValueReturnSuccess : ReturnSuccessBase
    {
        public DebugValueReturnSuccess(JsonRpcValue value) : base(value) { }

        [JsonPropertyName("DebugValue")]
        public new JsonRpcValue Value => base.Value;
    }

    public class ActionReturnSuccess : IReturnSuccess
    {
        [JsonPropertyName("Action")]
        public object Action => null;
    }
}
