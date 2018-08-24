using System;


namespace Nero
{
    /// <summary>
    /// Represents the symbol type in the target language.
    /// </summary>
    class Symbol : IValue
    {
        public Symbol(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Content of the symbol.
        /// </summary>
        public string Text { get; private set; }

        public string Represent()
        {
            return "'" + Text;
        }
    }
}
