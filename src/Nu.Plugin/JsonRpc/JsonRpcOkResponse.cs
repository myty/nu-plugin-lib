namespace Nu.Plugin
{
    internal class JsonRpcOkResponse : JsonRpcResponse
    {
        public JsonRpcOkResponse(object okResponseParams) => Params = new
        {
            Ok = okResponseParams
        };

        public override object Params { get; }
    }
}