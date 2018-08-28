using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    abstract class Continuation
    {
        public abstract Bounce Apply(IValue value);

        protected Continuation Super { get; } = null;
    }
}
