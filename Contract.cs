using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    // This class is mainly used by the Contract class.
    // Consider defining it as a private nested class of the Contract class.
    /// <summary>
    /// Represents the arity of a procedure.
    /// </summary>
    class Arity
    {
        public enum Mode { Equal, GreaterEqual }

        public Arity(int number, Mode mode)
        {
            Number = number;
            ArityMode = mode;
        }

        public int Number { get; }

        public Mode ArityMode { get; }

        public bool Verify(int argsCount)
        {
            switch (ArityMode)
            {
                case Mode.Equal:
                    return Number == argsCount;
                case Mode.GreaterEqual:
                    return argsCount >= Number;
                default:
                    throw new Exception($"Unknown arity mode: {ArityMode}");
            }
        }

        public string Representation
        {
            get
            {
                switch (ArityMode)
                {
                    case Mode.Equal:
                        return Number.ToString();
                    case Mode.GreaterEqual:
                        return $">={Number}";
                    default:
                        throw new Exception($"Unknown arity mode: {ArityMode}");
                }
            }
        }
        
    }

    /// <summary>
    /// Represents the signature contract of a procedure.
    /// It checks the arity and types of the incoming arguments
    /// of a procedure against its signature, which is specified 
    /// by the definition/declaration of the procedure.
    /// (Currently, this class only applies to the primitive procedures.)
    /// </summary>
    class Contract
    {

        public Contract(Arity arity, IList<Type> paramTypes)
        {
            if (!arity.Verify(paramTypes.Count))
                // The Contract class is designed to be used by the primitive procedures only, 
                // which means that I should take care of getting their contracts & implementation right.
                // If compound procedures are also allowed to specify their contracts, then 
                // this exception should be replaced with an interpreter-handled exception.
                throw new ArgumentException("The given parameter types does not match the specified arity.");

            if (paramTypes.Where((t) => !Utils.IsIValueType(t)).Count() != 0)
                throw new ArgumentException("The given parameter types contain a non-IValue type.");

            this.arity = arity;
            this.paramTypes = paramTypes;
        }

        private Arity arity;

        private IList<Type> paramTypes;

        /// <summary>
        /// Checks the arity and types of the incoming arguments against the specification.
        /// A return value indicates whether the incoming arguments are valid. If False is
        /// returned, the out parameter will be assigned a string describing the violation.
        /// </summary>
        /// <param name="arguments">The incoming arguments to check.</param>
        /// <param name="msg">Set to null if this method returns True. Otherwise, it
        /// contains description about the violation of the contract.</param>
        /// <returns></returns>
        public bool TryVerify(IReadOnlyCollection<IValue> arguments, out string msg)
        {
            var argTypes = (from arg in arguments select arg.GetType()).ToArray();

            List<int> LocateTypeMismatches()
            {
                // Compare each argument with the corresponding parameter
                // and check their type compatibility
                // Record the index of any type mismatch

                var indices = new List<int>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    Type argType = argTypes[i];
                    Type paramType = (i < paramTypes.Count) ? paramTypes[i] : paramTypes[paramTypes.Count - 1];
                    if (!paramType.IsAssignableFrom(argType))
                    {
                        indices.Add(i);
                    }
                }

                return indices;
            }

            (bool, string) ReportTypeMismatches(List<int> indices)
            {
                string msg1;

                // If there is any type mismatch, report with the message
                // argument and return False
                if (indices.Count != 0)
                {
                    var builder = new StringBuilder();
                    foreach (int i in indices)
                    {
                        int paramIndex = (i < paramTypes.Count) ? i : paramTypes.Count - 1;
                        string correctType = Utils.IValueTypeToString(paramTypes[paramIndex]);
                        string givenType = Utils.IValueTypeToString(argTypes[i]);
                        builder.Append($"\n\t(arg #{i + 1}) Expected: {correctType}. Given: {givenType}");
                    }

                    msg1 = "Type mismatch:" + builder.ToString();
                    return (false, msg1);
                }
                else
                {
                    msg1 = null;
                    return (true, msg1);
                }
            }

            //// Part I - Verify arity
            if (!arity.Verify(arguments.Count))
            {
                msg = $"Arity mismatch. Expected: {arity.Representation}. Given: {arguments.Count}";
                return false;
            }

            //// Part II - Verify types

            bool result;
            var mismatchIndices = LocateTypeMismatches();
            (result, msg) = ReportTypeMismatches(mismatchIndices);
            return result;
        }
    }
}
