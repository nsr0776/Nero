using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    class Bounce
    {
        public Bounce(IValue value)
        {
            this.value = value;
        }

        public Bounce(Func<Bounce> wrapper)
        {
            this.wrapper = wrapper;
        }

        private IValue value = null;

        private Func<Bounce> wrapper = null;

        public bool IsFinalAnswer { get => value != null; }

        public Bounce Next
        {
            get
            {
                if (IsFinalAnswer)
                    throw new Exception("Bounce.Next called on a final answer.");

                return wrapper.Invoke();
            }
        }

        public static IValue Trampoline(Bounce initialBounce)
        {
            Bounce bounce = initialBounce;
            while (!bounce.IsFinalAnswer)
            {
                bounce = bounce.Next;
            }

            return bounce.value;
        }
    }
}
