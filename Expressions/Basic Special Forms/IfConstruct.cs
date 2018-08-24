using System;


namespace Nero
{
    /// <summary>
    /// Models the "if" conditional construct in the target language.
    /// A basic special form.
    /// </summary>
    class IfConstruct : SpecialForm
    {
        public IfConstruct(Expression predicate, Expression consequent, Expression alternative = null)
        {
            Predicate = predicate;
            Consequent = consequent;
            Alternative = alternative;
        }

        public Expression Predicate { get; private set; }

        public Expression Consequent { get; private set; }

        public Expression Alternative { get; private set; }

        public static string KeyWord { get; } = "if";

        public override IValue Evaluate(Environment env)
        {
            IValue predValue = Predicate.Evaluate(env);
            if (Boolean.IsTrue(predValue))
            {
                return Consequent.Evaluate(env);
            }
            else if (Alternative == null)
            {
                return Boolean.FalseLiteral;
            }
            else
            {
                return Alternative.Evaluate(env);
            }
        }

        public static IfConstruct Analyze(Parsing.SList expr)
        {
            if (expr.Count != 3 && expr.Count != 4)
                    throw new BadSyntaxException(KeyWord, string.Empty, expr.Text);

            Expression predicate = Parsing.Analyzer.Analyze(expr[1]);
            Expression consequent = Parsing.Analyzer.Analyze(expr[2]);
            Expression alternative = null;
            if (expr.Count == 4)
                alternative = Parsing.Analyzer.Analyze(expr[3]);

            return new IfConstruct(predicate, consequent, alternative);
        }
    }
}
