using System;


namespace Nero
{
    /// <summary>
    /// Represents the void value in the target language. A singleton class.
    /// </summary>
    class Void : IValue
    {
        /// <summary>
        /// A singleton instance representing the void literal.
        /// </summary>
        public static readonly Void VoidLiteral;

        static Void()
        {
            VoidLiteral = new Void();
        }

        private Void()
        { }

        public string Represent()
        {
            return "#<void>";
        }
    }
}
