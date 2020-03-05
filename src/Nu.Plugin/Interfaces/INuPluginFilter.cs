namespace Nu.Plugin.Interfaces
{
    public interface INuPluginFilter
    {
        object        BeginFilter();
        JsonRpcParams Filter(JsonRpcParams requestParams);
        object        EndFilter();
    }
}
