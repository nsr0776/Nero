using System;
using System.Collections.Generic;
using System.Linq;


namespace Nero
{
    /// <summary>
    /// Models the sequence expression in the target language.
    /// A basic special form.
    /// </summary>
    class Sequence : SpecialForm
    {
        public Sequence(IEnumerable<Expression> bodyExpressions)
        {
            BodyExpressions = (from expr in bodyExpressions select expr).ToList();
        }

        public IList<Expression> BodyExpressions { get; private set; }

        public static string KeyWord { get; } = "begin";

        public override IValue Evaluate(Environment env)
        {
            IValue result = Void.VoidLiteral;

            foreach (var expr in BodyExpressions)
            {
                result = expr.Evaluate(env);
            }

            return result;
        }

        public static Sequence Analyze(Parsing.SList expr)
        {
            var bodyExpressions = (from sExpr in expr.Skip(1)
                                   select Parsing.Analyzer.Analyze(sExpr)).ToList();
            return new Sequence(bodyExpressions);
        }
    }
}
