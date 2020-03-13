using System.Text.Json.Serialization;

namespace Nu.Plugin.JsonRpc
{
    public abstract class Result
    {
        public Result(object result) => Value = result;

        [JsonIgnore]
        public object Value { get; }
    }
    public abstract class Result<T> : Result
    {
        public Result(T result) : base(result) { }

        [JsonIgnore]
        public new T Value => (T)base.Value;
    }

    public class OkResult : Result<object>
    {
        public OkResult(object result) : base(result) { }

        [JsonPropertyName("Ok")]
        public new object Value => base.Value;
    }

    public class OkResult<T> : Result<T>
    {
        public OkResult(T result) : base(result) { }

        [JsonPropertyName("Ok")]
        public new T Value => base.Value;
    }

    public class ErrResult : Result
    {
        public ErrResult(object result) : base(result) { }

        [JsonPropertyName("Err")]
        public new object Value => base.Value;
    }
}
