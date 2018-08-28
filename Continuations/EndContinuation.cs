using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    class EndContinuation : Continuation
    {
        public EndContinuation() { }

        public override Bounce Apply(IValue value)
        {
            Func<Bounce> wrapper = () =>
            {
                return new Bounce(value);
            };

            return new Bounce(wrapper);
        }
    }
}
