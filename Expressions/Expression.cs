using System;


namespace Nero
{
    /// <summary>
    /// Models the the target language expression.
    /// Everything in the target language is essentially an expression.
    /// </summary>
    abstract class Expression
    {
        /// <summary>
        /// Evaluates the expression in the provided environment.
        /// </summary>
        /// <param name="env">The environment for evaluation.</param>
        /// <returns></returns>
        public abstract Bounce Evaluate(Environment env, Continuation cont);
    }
}
