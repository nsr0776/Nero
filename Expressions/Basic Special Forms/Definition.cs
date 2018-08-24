using System;
using System.Linq;

using Nero.Parsing;


namespace Nero
{
    /// <summary>
    /// Models the definition expression in the target language.
    /// A basic special form.
    /// </summary>
    class Definition : SpecialForm
    {
        public Definition(string variable, Expression valueExpression)
        {
            Variable = variable;
            ValueExpression = valueExpression;
        }

        public string Variable { get; private set; }

        public Expression ValueExpression { get; private set; }

        public static string KeyWord { get; } = "define";

        public override IValue Evaluate(Environment env)
        {
            IValue value = ValueExpression.Evaluate(env);
            env.AddBinding(Variable, value);

            return Void.VoidLiteral;
        }

        public static Definition Analyze(SList expr)
        {
            if (expr[1] is SAtom varExpr)
            {
                // Example: (define foo (+ 42 42))
                if (expr.Count != 3)
                    throw new BadSyntaxException(KeyWord, string.Empty, expr.Text);

                string variable = varExpr.Text;
                Expression valueExpression = Analyzer.Analyze(expr[2]);
                return new Definition(variable, valueExpression);
            }
            else
            {
                // Example (define (foo x y) (+ x y))

                var procNameAndParams =
                     from item in ((SList)expr[1]).Cast<SAtom>()
                     select item.Text;

                string procName = procNameAndParams.First();
                var procParams = procNameAndParams.Skip(1);
                var procBody = from sExpr in expr.Skip(2) select Analyzer.Analyze(sExpr);

                Expression lambda = new Lambda(procParams, procBody);
                return new Definition(procName, lambda);
            }
        }
    }
}
