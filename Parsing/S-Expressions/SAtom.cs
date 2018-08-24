using System;


namespace Nero.Parsing
{
    /// <summary>
    /// Models the atomic S-Expressions.
    /// </summary>
    class SAtom : SExpression
    {
        public SAtom(string text)
        {
            this.text = text;
        }

        private readonly string text;

        /// <summary>
        /// Text representation of the S-Atom.
        /// </summary>
        public override string Text { get => text; }
    }
}
