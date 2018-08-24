using System;
using System.Collections.Generic;
using System.Linq;

using Nero.Parsing;


namespace Nero
{
    /// <summary>
    /// Models the "cond" conditional construct in the target language.
    /// </summary>
    class CondConstruct : SpecialForm
    {
        public class CondClause
        {
            public CondClause(Expression predicate, Expression consequent, bool isElseClause = false)
            {
                Predicate = predicate;
                Consequent = consequent;
                IsElseClause = isElseClause;
            }

            public Expression Predicate { get; private set; }

            public Expression Consequent { get; private set; }

            public bool IsElseClause { get; private set; }

            public static CondClause Analyze(SList expr)
            {
                if (expr.Count < 2)
                    throw new BadSyntaxException(KeyWord, "Clauses should be lists of length 2 or greater", expr.Text);

                if (expr[0] is SAtom sa && sa.Text == "else")
                {
                    Expression consequent = Analyzer.Analyze(expr[1]);
                    return new CondClause(null, consequent, true);
                }
                else
                {
                    Expression predicate = Analyzer.Analyze(expr[0]);
                    Sequence consequent = new Sequence(
                            from sExpr in expr.Skip(1) select Analyzer.Analyze(sExpr)
                            );

                    return new CondClause(predicate, consequent);
                }
            }
        }

        public CondConstruct(IEnumerable<CondClause> condClauses)
        {
            CondClauses = (from clause in condClauses select clause).ToList();
        }

        public IList<CondClause> CondClauses { get; private set; }

        public static string KeyWord { get; } = "cond";

        public override IValue Evaluate(Environment env)
        {
            foreach (var clause in CondClauses)
            {
                if (clause.IsElseClause)
                {
                    return clause.Consequent.Evaluate(env);
                }
                else
                {
                    IValue predVal = clause.Predicate.Evaluate(env);
                    if (Boolean.IsTrue(predVal))
                    {
                        return clause.Consequent.Evaluate(env);
                    }
                }
            }
            return Void.VoidLiteral;
        }


        public static CondConstruct Analyze(SList expr)
        {
            var clauses = new List<CondClause>();
            for (int i = 1; i < expr.Count; i++)
            {
                var sExpr = expr[i];
                if (sExpr is SList sl)
                {
                    var clause = CondClause.Analyze(sl);
                    if (clause.IsElseClause && i != expr.Count - 1)
                        throw new BadSyntaxException(KeyWord, "ELSE clause is not the last");

                    clauses.Add(clause);
                }
                else
                    throw new BadSyntaxException(KeyWord, "Invalid clause", expr.Text);
            }

            return new CondConstruct(clauses);
        }

    }
}
