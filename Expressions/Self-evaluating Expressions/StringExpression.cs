using System;


namespace Nero
{
    /// <summary>
    /// Models a string expression in the target language.
    /// (e.g. "hello world")
    /// </summary>
    class StringExpression : SelfEvaluating
    {
        public StringExpression(string literal)
        {
            this.literal = literal;
        }

        private readonly string literal;

        public override IValue Evaluate(Environment env)
        {
            return new String(literal);
        }

        public static StringExpression Analyze(Parsing.SAtom expr)
        {
            string text = expr.Text;
            string literal = text.Substring(1, text.Length - 2);    // Discards the opening & closing double quotes
            return new StringExpression(literal);
        }

        public static bool IsInstance(Parsing.SAtom expr)
        {
            string text = expr.Text;
            return text.Length >= 2 && text[0] == '\"' && text[text.Length - 1] == '\"';
        }
    }
}
