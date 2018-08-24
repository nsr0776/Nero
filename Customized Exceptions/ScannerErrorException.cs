using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    [Serializable]
    class ScannerErrorException : InterpreterException
    {
        public ScannerErrorException(string message) : base("scanner", message) { }
    }
}
