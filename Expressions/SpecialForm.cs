using System;
using System.Collections.Generic;

using Nero.Parsing;

namespace Nero
{
    /// <summary>
    /// Models the special forms in the target language.
    /// </summary>
    abstract class SpecialForm : Expression
    {
        /// <summary>
        /// A table containing all the special forms.
        /// </summary>
        public static IDictionary<string, Func<SList, SpecialForm>> Table { get; } = 
            new Dictionary<string, Func<SList, SpecialForm>>();
    }
}
