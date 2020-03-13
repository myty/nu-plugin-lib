using System.Text.Json.Serialization;

namespace Nu.Plugin
{
    public interface IPositionalType { }

    public class MandatoryPostionalType : IPositionalType
    {
        public MandatoryPostionalType(string name) : this(name, SyntaxShape.Any) { }
        public MandatoryPostionalType(string name, SyntaxShape syntax) => Mandatory = new string[] { name, syntax.Shape };

        [JsonPropertyName("Mandatory")]
        public string[] Mandatory { get; }
    }

    public class OptionalPostionalType : IPositionalType
    {
        public OptionalPostionalType(string name) : this(name, SyntaxShape.Any) { }
        public OptionalPostionalType(string name, SyntaxShape syntax) => Optional = new string[] { name, syntax.Shape };

        [JsonPropertyName("Optional")]
        public string[] Optional { get; }
    }
}
