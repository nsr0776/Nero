using System;


namespace Nero
{
    /// <summary>
    /// Represents the string data type in the target language.
    /// </summary>
    class String : IValue
    {
        public String(string str)
        {
            UnderlyingString = str;
        }

        public String(String str)
        {
            UnderlyingString = str.UnderlyingString;
        }

        public string UnderlyingString { get; private set; }

        public string Represent()
        {
            return UnderlyingString;
        }
    }
}
