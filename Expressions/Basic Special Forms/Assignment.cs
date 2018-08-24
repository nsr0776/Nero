using System;

using Nero.Parsing;

namespace Nero
{
    /// <summary>
    /// Models the assignment expression in the target language.
    /// A basic special form.
    /// </summary>
    class Assignment : SpecialForm
    {
        public Assignment(string variable, Expression newValueExpression)
        {
            Variable = variable;
            NewValueExpression = newValueExpression;
        }

        public string Variable { get; private set; }

        public Expression NewValueExpression { get; private set; }

        public static string KeyWord { get; } = "set!";

        public override IValue Evaluate(Environment env)
        {
            IValue value = NewValueExpression.Evaluate(env);
            env.SetBinding(Variable, value);

            return Void.VoidLiteral;
        }

        public static Assignment Analyze(SList expr)
        {
            if (expr.Count != 3)
                throw new BadSyntaxException(KeyWord, string.Empty, expr.Text);

            string variable = expr[1].Text;
            Expression newValueExpression = Analyzer.Analyze(expr[2]);
            return new Assignment(variable, newValueExpression);
        }
    }
}
