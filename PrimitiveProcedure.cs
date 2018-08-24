using System;
using System.Collections.Generic;


namespace Nero
{
    /// <summary>
    /// An OOP wrapper for the primitive procedures.
    /// </summary>
    class PrimitiveProcedure : IValue
    {
        /// <summary>
        /// A table containing all the primitive procedures.
        /// </summary>
        public static IDictionary<string, PrimitiveProcedure> Table = new Dictionary<string, PrimitiveProcedure>();

        public PrimitiveProcedure(string name, Contract contract, Func<IReadOnlyCollection<IValue>, IValue> underlyingFunction)
        {
            Name = name;
            Contract = contract;
            this.underlyingFunction = underlyingFunction;
        }

        public string Name { get; }

        public Contract Contract { get; }

        public IValue Execute(IReadOnlyCollection<IValue> arguments)
        {
            if (!Contract.TryVerify(arguments, out string message))
            {
                throw new ContractViolationException(Name, message);
            }

            return underlyingFunction.Invoke(arguments);
        }

        private Func<IReadOnlyCollection<IValue>, IValue> underlyingFunction;

        public string Represent()
        {
            return "#<primitive>";
        }
    }
}
