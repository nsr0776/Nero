using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    [Serializable]
    class BadSyntaxException : InterpreterException
    {
        public BadSyntaxException(string specialFormName, string message) 
            : base($"{specialFormName}: bad syntax", message) { }

        public BadSyntaxException(string specialFormName, string message, string relatedCode)
            : base($"{specialFormName}: bad syntax", message, relatedCode) { }
    }
}
