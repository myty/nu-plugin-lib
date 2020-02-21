namespace Nu.Plugin
{
    public class JsonRpcOkResponse : JsonRpcResponse
    {
        public JsonRpcOkResponse(object okResponseParams) => Params = new
        {
            Ok = okResponseParams
        };

        public override object Params { get; }
    }
}