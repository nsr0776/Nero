using System;
using System.Collections.Generic;
using System.Linq;

using Nero.Parsing;


namespace Nero
{
    /// <summary>
    /// Models the lambda expression in the target language.
    /// A basic special form.
    /// </summary>
    class Lambda : SpecialForm
    {
        public Lambda(IEnumerable<string> parameters, IEnumerable<Expression> body)
        {
            Parameters = parameters.Select((id) =>
            {
                if (Utils.IsValidIdentifier(id))
                    return id;
                else
                    throw new BadSyntaxException(KeyWord, "Invalid identifier", id);
            }).ToList();
            Body = (from expr in body select expr).ToList();
        }

        public IList<string> Parameters;

        public IList<Expression> Body;

        public static string KeyWord { get; } = "lambda";

        public override IValue Evaluate(Environment env)
        {
            var proc = new CompoundProcedure(Parameters, Body, env);
            return proc;
        }
        
        public static Lambda Analyze(SList expr)
        {
            if (expr.Count < 3)
                throw new BadSyntaxException(KeyWord, string.Empty, expr.Text);

            var parameters =
                from item in ((SList)expr[1]).Cast<SAtom>()
                select item.Text;

            var body = from sExpr in expr.Skip(2) select Analyzer.Analyze(sExpr);

            return new Lambda(parameters, body);
        }
    }
}
