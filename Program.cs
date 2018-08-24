using System;
using System.Linq;
using System.Collections.Generic;

using Nero.Parsing;

namespace Nero
{
    class Program
    {
        static void RegisterPrimitiveProcedures()
        {
            var unaryArity = new Arity(1, Arity.Mode.Equal);
            var binaryArity = new Arity(2, Arity.Mode.Equal);
            var nonEmptyArity = new Arity(1, Arity.Mode.GreaterEqual);

            var arithContract = new Contract(nonEmptyArity, new List<Type> { typeof(Number) });
            var binNumContract = new Contract(binaryArity, new List<Type> { typeof(Number), typeof(Number) });
            var unaryIValueContract = new Contract(unaryArity, new List<Type> { typeof(IValue) });
            var binaryIValueContract = new Contract(binaryArity, new List<Type> { typeof(IValue), typeof(IValue) });
            var mpairContract = new Contract(unaryArity, new List<Type> { typeof(MPair) });
            var listContract = new Contract(nonEmptyArity, new List<Type> { typeof(IValue) });

            var ppTable = new (string, Contract, Func<IReadOnlyCollection<IValue>, IValue>)[]
            {
                ("+", arithContract, PrimitiveImplementation.Add),
                ("-", arithContract, PrimitiveImplementation.Sub),
                ("*", arithContract, PrimitiveImplementation.Mul),
                ("remainder", binNumContract, PrimitiveImplementation.Remainder),
                ("=", binNumContract, PrimitiveImplementation.NumericEqual),
                ("<", binNumContract, PrimitiveImplementation.NumericLessThan),
                ("<=", binNumContract, PrimitiveImplementation.NumericLessEqual),
                (">", binNumContract, PrimitiveImplementation.NumericGreaterThan),
                (">=", binNumContract, PrimitiveImplementation.NumericGreaterEqual),
                ("cons", binaryIValueContract, PrimitiveImplementation.Cons),
                ("car", mpairContract, PrimitiveImplementation.Car),
                ("cdr", mpairContract, PrimitiveImplementation.Cdr),
                ("list", listContract, PrimitiveImplementation.MakeList),
                ("true?", unaryIValueContract, PrimitiveImplementation.IsTrue),
                ("false?", unaryIValueContract, PrimitiveImplementation.IsFalse),
                ("null?", unaryIValueContract, PrimitiveImplementation.IsNull),
                ("pair?", unaryIValueContract, PrimitiveImplementation.IsPair),
                ("list?", unaryIValueContract, PrimitiveImplementation.IsList),
                ("display", unaryIValueContract, PrimitiveImplementation.Display)
            };

            foreach (var record in ppTable)
            {
                var pp = new PrimitiveProcedure(name: record.Item1,
                                                contract: record.Item2,
                                                underlyingFunction: record.Item3);
                PrimitiveProcedure.Table.Add(record.Item1, pp);
            }
        }

        static void RegisterBuiltinSpecialForms()
        {
            var sfTable = new Dictionary<string, Func<SList, SpecialForm>>
            {
                {Sequence.KeyWord, Sequence.Analyze},
                {Quotation.KeyWord, Quotation.Analyze},
                {Lambda.KeyWord, Lambda.Analyze},
                {Definition.KeyWord, Definition.Analyze},
                {Assignment.KeyWord, Assignment.Analyze},
                {IfConstruct.KeyWord, IfConstruct.Analyze},
                {CondConstruct.KeyWord, CondConstruct.Analyze},
                {LetConstruct.KeyWord, LetConstruct.Analyze }
            };

            foreach (var binding in sfTable)
            {
                SpecialForm.Table.Add(binding.Key, binding.Value);
            }
        }

        static void Initialize()
        {
            RegisterPrimitiveProcedures();
            RegisterBuiltinSpecialForms();
        }

        static void Main(string[] args)
        {
            //Test();
            Initialize();

            var repl = new DriverLoop();
            repl.Run();
        }

        static void Test()
        {
            var a = new List<int> { 3, 4, 7 };
            
        }
    }

}
