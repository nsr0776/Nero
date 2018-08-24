using System;
using System.Collections.Generic;
using System.Linq;

using Nero.Parsing;

namespace Nero
{
    /// <summary>
    /// The Read-Eval-Print Loop.
    /// </summary>
    class DriverLoop
    {
        void RegisterPredefinedVariables()
        {
            // TODO: add some predefined variables/procedures here (e.g. map filter)
            var bindings = new Dictionary<string, IValue>
            {
                {"nil", MPair.Nil }
            };

            foreach (var binding in bindings)
            {
                theGlobalEnvironment.AddBinding(binding.Key, binding.Value);
            }
        }

        public DriverLoop()
        {
            // Setup the global environment
            IDictionary<string, PrimitiveProcedure> primitives = PrimitiveProcedure.Table;
            var names = from binding in primitives select binding.Key;
            var procs = from binding in primitives select binding.Value;
            theGlobalEnvironment = new Environment(Environment.TheEmptyEnvironment, names, procs);

            RegisterPredefinedVariables();
        }

        void PromptForInput()
        {
            Console.Write(">>> ");
        }

        /// <summary>
        /// Formats and prints the result value of an evaluation.
        /// </summary>
        /// <param name="output"></param>
        void AnnounceOutput(string output)
        {
            Console.WriteLine($"output: {output}");
        }

        /// <summary>
        /// Main process of the REPL.
        /// </summary>
        public void Run()
        {
            do
            {
                PromptForInput();
                string input = Console.ReadLine();

                try
                {
                    var expressions =
                        from sExpr in Scanner.Read(input)
                        select Analyzer.Analyze(sExpr);

                    foreach (var expr in expressions)
                    {
                        IValue result = expr.Evaluate(theGlobalEnvironment);
                        AnnounceOutput(result.Represent());
                    }
                }
                catch (InterpreterException ex)
                {
                    Console.WriteLine("An exception occurred:");

                    Console.WriteLine($"[{ex.Source}] {ex.Description}");

                    if (ex.RelatedCode != null)
                    {
                        Console.WriteLine($"Related code: {ex.RelatedCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected exception occurred.\n{ex.ToString()}");
                    break;
                }

            } while (true);
        }

        public string EvalOne(Expression expr)
        {
            IValue result = expr.Evaluate(theGlobalEnvironment);
            return result.Represent();
        }

        /// <summary>
        /// The global environment for the entire evaluator. 
        /// It is the base environment for any other environment
        /// spawned during the execution of the REPL.
        /// </summary>
        private readonly Environment theGlobalEnvironment;
    }
}
