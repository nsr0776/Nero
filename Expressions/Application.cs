using System;
using System.Collections.Generic;
using System.Linq;


namespace Nero
{
    /// <summary>
    /// Models a procedure application expression.
    /// </summary>
    class Application : Expression
    {
        public Application(Expression procedureExpression, IEnumerable<Expression> arguments)
        {
            ProcedureExpression = procedureExpression;
            ArgumentExpressions = (from arg in arguments select arg).ToList();
        }

        public Expression ProcedureExpression { get; private set; }

        public IList<Expression> ArgumentExpressions { get; private set; }

        /// <summary>
        /// Implements applicative-order evaluation.
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public override IValue Evaluate(Environment env)
        {
            // Eval and get the operator
            IValue procedure = ProcedureExpression.Evaluate(env);

            // Both primitive & compound procedures are strict
            var arguments = from argExpr in ArgumentExpressions
                            select argExpr.Evaluate(env);

            switch (procedure)
            {
                case PrimitiveProcedure pproc:
                    return pproc.Execute(arguments.ToList());
                case CompoundProcedure cproc:
                    return cproc.Execute(arguments.ToList());
                default:
                    throw new RuntimeErrorException("procedure application", $"Expected a procedure that can be applied. Given: {procedure.Represent()}");
            }
        }

        public static Application Analyze(Parsing.SList expr)
        {
            var car = expr[0];
            var cdr = expr.Skip(1);

            var procExpression = Parsing.Analyzer.Analyze(car);
            var args = from arg in cdr select Parsing.Analyzer.Analyze(arg);

            return new Application(procExpression, args);
        }
    }
}
