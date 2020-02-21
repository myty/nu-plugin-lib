namespace Nu.Plugin
{
    internal class JsonRpcValueResponse : JsonRpcResponse
    {
        public JsonRpcValueResponse(JsonRpcParams rpcParams) => Params = new
        {
            Ok = new object[] {
                new {
                    Ok = new {
                        Value = rpcParams
                    }
                }
            }
        };

        public override object Params { get; }
    }
}