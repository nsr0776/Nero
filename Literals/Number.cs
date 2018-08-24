using System;
using System.Numerics;


namespace Nero
{
    /// <summary>
    /// Represents the number data type in the target language.
    /// (Currently, it is implemented by directly mapping to the .NET class
    /// System.Numerics.BigInteger. Therefore, only integer arithmetics 
    /// is supported.)
    /// </summary>
    class Number : IValue
    {
        public Number(Number number)
        {
            UnderlyingNumber = number.UnderlyingNumber;
        }

        public Number(BigInteger bigInteger)
        {
            UnderlyingNumber = bigInteger;
        }

        public BigInteger UnderlyingNumber { get; set; }

        public string Represent()
        {
            return UnderlyingNumber.ToString();
        }
    }
}
