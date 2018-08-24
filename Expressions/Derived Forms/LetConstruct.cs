using System;
using System.Collections.Generic;
using System.Linq;

using Nero.Parsing;


namespace Nero
{
    class LetConstruct : SpecialForm
    {
        public LetConstruct(IEnumerable<string> parameters, 
            IEnumerable<Expression> argumentsExpressions,
            IEnumerable<Expression> body)
        {
            Parameters = (from param in parameters select param).ToList();
            ArgumentExpressions = (from argExpr in argumentsExpressions select argExpr).ToList();
            Body = (from expr in body select expr).ToList();
        }


        public IList<string> Parameters { get; private set; }

        public IList<Expression> ArgumentExpressions { get; private set; }

        public IList<Expression> Body { get; private set; }

        public static string KeyWord { get; } = "let";

        public override IValue Evaluate(Environment env)
        {
            var argumentValues = from expr in ArgumentExpressions select expr.Evaluate(env);
            var subEnv = new Environment(env, Parameters, argumentValues);

            IValue result = Void.VoidLiteral;   // let construct with an empty body returns void
            foreach (var expr in Body)
            {
                result = expr.Evaluate(subEnv);
            }

            return result;
        }

        public static LetConstruct Analyze(SList expr)
        {
            if (expr.Count < 3)
                throw new BadSyntaxException(KeyWord, "Missing bindings or body", expr.Text);


            var bindings = (SList)expr[1];
            var variables = new List<string>();
            var values = new List<Expression>();
            foreach (var binding in bindings)
            {
                if (binding is SList bd)
                {
                    if (bd.Count != 2)
                        throw new BadSyntaxException(KeyWord, "Bindings should be lists of length 2");

                    var variable = bd[0];
                    var value = bd[1];

                    if (variable is SAtom sa)
                    {
                        if (!Utils.IsValidIdentifier(sa.Text))
                            throw new BadSyntaxException(KeyWord, $"{variable.Text} is not a valid identifier");

                        variables.Add(sa.Text);
                        values.Add(Analyzer.Analyze(value));
                    }
                    else
                        throw new BadSyntaxException(KeyWord, "Variable name is not an identifier");
                }
                else
                    throw new BadSyntaxException(KeyWord, "Non-list for a binding");
            }

            var body = from sExpr in expr.Skip(2) select Analyzer.Analyze(sExpr);
            return new LetConstruct(variables, values, body);
        }
    }
}
