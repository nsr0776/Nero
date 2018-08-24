using System;


namespace Nero
{
    /// <summary>
    /// Represents a value that is the result of an evaluation.
    /// </summary>
    interface IValue
    {
        /// <summary>
        /// Analogous to 'ToString()'. Used by the target language.
        /// </summary>
        /// <returns></returns>
        string Represent();
    }
}
