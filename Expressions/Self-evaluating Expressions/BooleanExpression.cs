using System;


namespace Nero
{
    /// <summary>
    /// Models a boolean expression in the target language.
    /// (e.g. #t)
    /// </summary>
    class BooleanExpression : SelfEvaluating
    {
        public BooleanExpression(bool literal)
        {
            this.literal = literal;
        }

        private readonly bool literal;

        public override IValue Evaluate(Environment env)
        {
            return (literal) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static Expression Analyze(Parsing.SAtom sExpr)
        {
            if (sExpr.Text == "#t")
            {
                return new BooleanExpression(true);
            }
            else if (sExpr.Text == "#f")
            {
                return new BooleanExpression(false);
            }
            // This case should have been intercepted before entering this function
            else
                throw new ArgumentException($"Unknown boolean literal: {sExpr.Text}");
        }

        public static bool IsInstance(Parsing.SAtom sExpr)
        {
            return sExpr.Text == "#t" || sExpr.Text == "#f";
        }
    }
}
