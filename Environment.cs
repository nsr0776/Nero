using System;
using System.Collections.Generic;
using System.Linq;


namespace Nero
{
    /// <summary>
    /// Represents the environment in "the Environment Model of Evaluation".
    /// </summary>
    class Environment
    {
        /// <summary>
        /// Internal implementation of the bindings table.
        /// </summary>
        private Dictionary<string, IValue> bindings = null;

        /// <summary>
        /// Constructs a new environment by extending an existing environment 
        /// with an initial list of bindings.
        /// </summary>
        /// <param name="enclosingEnvironment">The enclosing environment.</param>
        /// <param name="keys">The list of binding keys.</param>
        /// <param name="values">The list of binding values.</param>
        public Environment(Environment enclosingEnvironment, IEnumerable<string> keys, IEnumerable<IValue> values)
        {
            EnclosingEnvironment = enclosingEnvironment;

            bindings = keys.Zip(values, (k, v) => new { Key = k, Value = v })
                           .ToDictionary((x) => x.Key, (x) => x.Value);
        }

        /// <summary>
        /// Static ctor
        /// </summary>
        static Environment()
        {
            TheEmptyEnvironment = new Environment(null, new List<string>(), new List<IValue>());
        }

        /// <summary>
        /// A literal representing the empty environment, which is supposed to be the enclosing 
        /// environment of the global environment.
        /// </summary>
        public static readonly Environment TheEmptyEnvironment;

        /// <summary>
        /// Defines a new variable. Shadows variables with the same name in enclosing environments.
        /// Signals an error when redefining an existing variable.
        /// </summary>
        /// <param name="key">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void AddBinding(string key, IValue value)
        {
            if (bindings.ContainsKey(key))
            {
                throw new RuntimeErrorException("environment", $"Attempting to redefine '{key}");
            }

            bindings.Add(key, value);
        }

        /// <summary>
        /// Assigns to a variable. Searches the enclosing environment if not found. 
        /// Signals an error if the variable is unbound.
        /// </summary>
        /// <param name="key">The name of the variable.</param>
        /// <param name="newValue">The new value of the variable</param>
        public void SetBinding(string key, IValue newValue)
        {
            if (this == TheEmptyEnvironment)
            {
                throw new RuntimeErrorException("environment", $" Attempting to assign to an unbound variable '{key}");
            }

            if (bindings.ContainsKey(key))
            {
                bindings[key] = newValue;
            }
            else
            {
                EnclosingEnvironment.SetBinding(key, newValue);
            }
        }

        /// <summary>
        /// References a variable. Searches the enclosing environment if not found.
        /// Signals an error if the variable is unbound.
        /// </summary>
        /// <param name="key">The name of the variable.</param>
        /// <returns></returns>
        public IValue LookupBinding(string key)
        {
            if (this == TheEmptyEnvironment)
            {
                throw new RuntimeErrorException("environment", $"Attempting to reference an unbound variable '{key}");
            }

            if (bindings.ContainsKey(key))
            {
                return bindings[key];
            }
            else
            {
                return EnclosingEnvironment.LookupBinding(key);
            }
        }

        public Environment EnclosingEnvironment { get; private set; }

        // The Environment class no longer implements the IValue
        // interface, so this method becomes obsolete. It is 
        // preserved for potential debug usages.
        string Represent()
        {
            string repr = "#<environment>";

            if (this == TheEmptyEnvironment)
                repr = "#<the-empty-env>";
            else if (EnclosingEnvironment == TheEmptyEnvironment)
                repr = "#<global-env>";

            return repr;
        }
    }
}
