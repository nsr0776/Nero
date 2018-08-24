using System;


namespace Nero
{
    /// <summary>
    /// Models the variable expression in the target language.
    /// </summary>
    class Variable : Expression
    {
        public Variable(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override IValue Evaluate(Environment env)
        {
            return env.LookupBinding(Name);
        }

        public static Variable Analyze(Parsing.SAtom expr)
        {
            if (Utils.IsValidIdentifier(expr.Text))
                return new Variable(expr.Text);
            else
                throw new BadSyntaxException("variable", "Invalid identifier", expr.Text);
        }
    }
}
