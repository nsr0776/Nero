using System;
using System.Collections.Generic;
using System.Linq;


namespace Nero
{
    /// <summary>
    /// Represents a compound procedure object. 
    /// It is built by a lambda expression.
    /// </summary>
    class CompoundProcedure : IValue
    {
        public CompoundProcedure(IEnumerable<string> parameters, IEnumerable<Expression> body, Environment environment)
        {
            Parameters = (from param in parameters select param).ToList();
            Body = body;
            Environment = environment;
        }

        public List<string> Parameters { get; private set; }

        public IEnumerable<Expression> Body { get; private set; }

        /// <summary>
        /// The environment in which this procedure object is defined.
        /// </summary>
        public Environment Environment { get; private set; }

        /// <summary>
        /// Executes the procedure with the provided arguments by sequentially
        /// evaluating all the body expressions in a new environment which extends
        /// the definition environment and contains all the parameter-argument
        /// bindings.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public IValue Execute(IReadOnlyCollection<IValue> arguments)
        {
            if (Parameters.Count != arguments.Count)
            {
                throw new RuntimeErrorException("compound procedure", $"Arity mismatch. Expected: {Parameters.Count}. Given: {arguments.Count}");
            }

            var subEnvironment = new Environment(Environment, Parameters, arguments);
            var bodySequence = new Sequence(Body);
            return bodySequence.Evaluate(subEnvironment);
        }

        public string Represent()
        {
            return "#<procedure>";
        }

    }
}
