using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    [Serializable]
    class RuntimeErrorException : InterpreterException
    {
        public RuntimeErrorException(string source, string message)
            : base(source, message, null) { }
    }
}
