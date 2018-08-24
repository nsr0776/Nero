using System;


namespace Nero
{
    /// <summary>
    /// Represents the boolean data type in the target language.
    /// </summary>
    class Boolean : IValue
    {
        private bool _value;

        private Boolean(bool value)
        {
            _value = value;
        }

        static Boolean()
        {
            TrueLiteral = new Boolean(true);
            FalseLiteral = new Boolean(false);
        }

        /// <summary>
        /// Judges whether an IValue is a true value in the target language's semantics.
        /// Returns true for any IValue other than #f.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsTrue(IValue obj)
        {
            return !IsFalse(obj);
        }

        /// <summary>
        /// Judges whether an IValue is a false value in the target language's semantics.
        /// Returns true if and only if the IValue is #f.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsFalse(IValue obj)
        {
            if (obj is Boolean boolean)
            {
                return boolean._value == false;
            }
            else
            {
                return false;
            }
        }

        public static readonly Boolean TrueLiteral;
        public static readonly Boolean FalseLiteral;


        public string Represent()
        {
            return (_value == true) ? "#t" : "#f";
        }
    }
}
