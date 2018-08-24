using System;


namespace Nero.Parsing
{
    /// <summary>
    /// Models the S-Expression.
    /// It is generated during parsing, and will be converted 
    /// into some proper Expression object by the analyze system.
    /// </summary>
    abstract class SExpression
    {
        /// <summary>
        /// Text representation of the S-Expression.
        /// </summary>
        public abstract string Text { get; }
    }
}
