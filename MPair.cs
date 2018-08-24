using System;


namespace Nero
{
    /// <summary>
    /// Represents the mutable pair construct in the target language,
    /// built by calling 'cons or 'list in the target language.
    /// </summary>
    class MPair : IValue
    {
        public MPair(IValue a, IValue b)
        {
            First = a;
            Rest = b;
        }

        /// <summary>
        /// The '() literal.
        /// </summary>
        public static readonly MPair Nil = new MPair(Void.VoidLiteral, Void.VoidLiteral); // [Caution] (pair? nil) should be #f, while (list? nil) should be #t

        /// <summary>
        /// Analogy to 'car'.
        /// </summary>
        public IValue First { get; set; }

        /// <summary>
        /// Analogy to 'cdr'.
        /// </summary>
        public IValue Rest { get; set; }

        public string Represent()
        {
            if (this == Nil)
                return "'()";
            else
                return $"(mcons {First.Represent()} {Rest.Represent()})";
        }
    }
}
