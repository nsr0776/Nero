using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    [Serializable]
    class ContractViolationException : RuntimeErrorException
    {
        public ContractViolationException(string procName, string violationMsg)
            : base($"procedure {procName}", violationMsg)
        { }
    }
}
