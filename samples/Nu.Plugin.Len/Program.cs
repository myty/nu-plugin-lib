using System.Threading.Tasks;

namespace Nu.Plugin.Len
{
    internal static class Program
    {
        private static async Task Main() => await NuPlugin
            .Build("len")
            .Description("Return the length of a string")
            .Switch("all", "All of everything")
            .Named("copy", SyntaxShape.Any, "")
            .FilterAsync<LengthFilter>();
    }
}
