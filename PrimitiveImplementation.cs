using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;


namespace Nero
{
    /// <summary>
    /// The underlying implementation functions for the primitive procedures.
    /// Common signature: "public static IValue foo(IReadOnlyCollection<IValue> arguments)"
    /// </summary>
    static class PrimitiveImplementation
    {
        public static IValue Add(IReadOnlyCollection<IValue> arguments)
        {
            var nums = from num in arguments.Cast<Number>() select num;
            var result = nums.Aggregate((lhs, rhs) => new Number(lhs.UnderlyingNumber + rhs.UnderlyingNumber));
            return result;
        }

        public static IValue Sub(IReadOnlyCollection<IValue> arguments)
        {
            if (arguments.Count == 1)
            {
                var num = (Number)arguments.First();
                return new Number(-num.UnderlyingNumber);
            }
            else
            {
                var nums = from num in arguments.Cast<Number>() select num;
                var result = nums.Aggregate((lhs, rhs) => new Number(lhs.UnderlyingNumber - rhs.UnderlyingNumber));
                return result;
            }
        }

        public static IValue Mul(IReadOnlyCollection<IValue> arguments)
        {
            var nums = from num in arguments.Cast<Number>() select num;
            var result = nums.Aggregate((lhs, rhs) => new Number(lhs.UnderlyingNumber * rhs.UnderlyingNumber));
            return result;
        }

        public static IValue Div(IReadOnlyCollection<IValue> arguments)
        {
            if (arguments.Count == 1)
            {
                var num = (Number)arguments.First();
                return new Number(1 / num.UnderlyingNumber);
            }
            else
            {
                var nums = from num in arguments.Cast<Number>() select num;
                var result = nums.Aggregate((lhs, rhs) => new Number(lhs.UnderlyingNumber / rhs.UnderlyingNumber));
                return result;
            }
        }

        public static IValue Remainder(IReadOnlyCollection<IValue> arguments)
        {
            var args = arguments.Cast<Number>().ToArray();
            Number lhs = args[0], rhs = args[1];
            var result = lhs.UnderlyingNumber % rhs.UnderlyingNumber;
            return new Number(result);
        }

        public static IValue NumericEqual(IReadOnlyCollection<IValue> arguments)
        {
            var args = arguments.Cast<Number>().ToArray();
            Number lhs = args[0], rhs = args[1];
            return (lhs.UnderlyingNumber == rhs.UnderlyingNumber) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue NumericLessThan(IReadOnlyCollection<IValue> arguments)
        {
            var args = arguments.Cast<Number>().ToArray();
            Number lhs = args[0], rhs = args[1];
            return (lhs.UnderlyingNumber < rhs.UnderlyingNumber) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue NumericLessEqual(IReadOnlyCollection<IValue> arguments)
        {
            var args = arguments.Cast<Number>().ToArray();
            Number lhs = args[0], rhs = args[1];
            return (lhs.UnderlyingNumber <= rhs.UnderlyingNumber) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue NumericGreaterThan(IReadOnlyCollection<IValue> arguments)
        {
            var args = arguments.Cast<Number>().ToArray();
            Number lhs = args[0], rhs = args[1];
            return (lhs.UnderlyingNumber > rhs.UnderlyingNumber) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue NumericGreaterEqual(IReadOnlyCollection<IValue> arguments)
        {
            var args = arguments.Cast<Number>().ToArray();
            Number lhs = args[0], rhs = args[1];
            return (lhs.UnderlyingNumber >= rhs.UnderlyingNumber) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue Cons(IReadOnlyCollection<IValue> arguments)
        {
            var args = arguments.ToArray();
            IValue a = args[0], b = args[1];

            return new MPair(a, b);
        }

        public static IValue Car(IReadOnlyCollection<IValue> arguments)
        {
            var arg = (MPair)arguments.First();
            return arg.First;
        }

        public static IValue Cdr(IReadOnlyCollection<IValue> arguments)
        {
            var arg = (MPair)arguments.First();
            return arg.Rest;
        }

        public static IValue IsNull(IReadOnlyCollection<IValue> arguments)
        {
            IValue arg = arguments.First();
            return (arg == MPair.Nil) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue IsList(IReadOnlyCollection<IValue> arguments)
        {
            if (arguments.First() is MPair pair)
            {
                while (pair != MPair.Nil && pair.Rest is MPair next)
                {
                    pair = next;
                }

                return (pair == MPair.Nil) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
            }
            else
                return Boolean.FalseLiteral;
        }

        public static IValue IsTrue(IReadOnlyCollection<IValue> arguments)
        {
            IValue arg = arguments.First();

            return (Boolean.IsTrue(arg)) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue IsFalse(IReadOnlyCollection<IValue> arguments)
        {
            IValue arg = arguments.First();

            return (Boolean.IsFalse(arg)) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue IsPair(IReadOnlyCollection<IValue> arguments)
        {
            IValue arg = arguments.First();

            return (arg is MPair mpair && mpair != MPair.Nil) ? Boolean.TrueLiteral : Boolean.FalseLiteral;
        }

        public static IValue MakeList(IReadOnlyCollection<IValue> arguments)
        {
            var result = (from arg in arguments.Append(MPair.Nil).Reverse()
                          select arg).Aggregate((list, item) => new MPair(item, list));

            return result;
        }

        public static IValue Display(IReadOnlyCollection<IValue> arguments)
        {
            Console.WriteLine($"{arguments.First().Represent()}");
            return Void.VoidLiteral;
        }
    }
}
