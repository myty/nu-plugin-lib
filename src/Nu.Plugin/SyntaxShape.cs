namespace Nu.Plugin
{
    public class SyntaxShape
    {
        private SyntaxShape(string shape) => Shape = shape;

        public string Shape { get; }

        /// Any syntactic form is allowed
        public static SyntaxShape Any => new SyntaxShape(nameof(Any));

        /// Strings and string-like bare words are allowed
        public static SyntaxShape String => new SyntaxShape(nameof(String));

        /// Values that can be the right hand side of a '.'
        public static SyntaxShape Member => new SyntaxShape(nameof(Member));

        /// A dotted path to navigate the table
        public static SyntaxShape ColumnPath => new SyntaxShape(nameof(ColumnPath));

        /// Only a numeric (integer or decimal) value is allowed
        public static SyntaxShape Number => new SyntaxShape(nameof(Number));

        /// A range is allowed (eg, `1..3`)
        public static SyntaxShape Range => new SyntaxShape(nameof(Range));

        /// Only an integer value is allowed
        public static SyntaxShape Int => new SyntaxShape(nameof(Int));

        /// A filepath is allowed
        public static SyntaxShape Path => new SyntaxShape(nameof(Path));

        /// A glob pattern is allowed, eg `foo*`
        public static SyntaxShape Pattern => new SyntaxShape(nameof(Pattern));

        /// A block is allowed, eg `{start this thing}`
        public static SyntaxShape Block => new SyntaxShape(nameof(Block));
    }
}
