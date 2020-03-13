using System.Threading.Tasks;

namespace Nu.Plugin.Len
{
    internal static class Program
    {
        private static async Task Main() => await NuPlugin
            .Build("len")
            .Description("Return the length of a string")
            .Required(SyntaxShape.String, "required_positional", "required positional description")
            .Optional(SyntaxShape.Int, "optional_positional_1", "optional positional description #1")
            .Optional(SyntaxShape.Any, "optional_positional_2", "optional positional description #2")
            .Switch("all", "All of everything")
            .Named(SyntaxShape.Any, "copy", "copy description")
            .RequiredNamed(SyntaxShape.String, "required", "required description")
            .Rest("test rest arguments")
            .FilterAsync<LengthFilter>();
    }
}
