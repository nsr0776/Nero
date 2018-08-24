using System;
using System.Numerics;


namespace Nero
{
    /// <summary>
    /// Models a number expression in the target language.
    /// (e.g. 42)
    /// </summary>
    class NumberExpression : SelfEvaluating
    {
        public NumberExpression(string literal)
        {
            this.literal = literal;
        }

        private readonly string literal;

        public override IValue Evaluate(Environment env)
        {
            var bi = BigInteger.Parse(literal);
            return new Number(bi);
        }

        public static NumberExpression Analyze(Parsing.SAtom expr)
        {
            return new NumberExpression(expr.Text);
        }

        public static bool IsInstance(Parsing.SAtom expr)
        {
            return BigInteger.TryParse(expr.Text, out _);
        }
    }
}
