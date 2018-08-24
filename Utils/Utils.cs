using System;
using System.Collections.Generic;


namespace Nero
{
    static class Utils
    {
        public static bool IsBracket(char ch)
        {
            return IsLeftBracket(ch) || IsRightBracket(ch);
        }

        public static bool IsLeftBracket(char ch)
        {
            return ch == '(' || ch == '[' || ch == '{';
        }

        public static bool IsRightBracket(char ch)
        {
            return ch == ')' || ch == ']' || ch == '}';
        }

        public static bool IsValidIdentifier(string identifier)
        {
            if (identifier.Length == 0)
                return false;
            if (char.IsDigit(identifier[0]))
                return false;

            return true;
        }

        public static List<IValue> MPairToList(MPair pair)
        {
            var result = new List<IValue>();

            while (pair != MPair.Nil)
            {
                result.Add(pair.First);
                if (pair.Rest is MPair rest)
                    pair = rest;
                else
                    throw new ArgumentException($"{pair} is not a well-formed list");
            }

            return result;
        }

        public static string IValueTypeToString(Type type)
        {
            if (!Utils.IsIValueType(type))
                throw new ArgumentException("The given Type object is not a subclass of the IValue interface.");

            var table = new Dictionary<Type, string>
            {
                {typeof(Boolean),  "<boolean>"},
                {typeof(Number),  "<number>"},
                {typeof(String),  "<string>"},
                {typeof(Symbol),  "<symbol>"},
                {typeof(MPair),  "<mpair>"},
                {typeof(PrimitiveProcedure),  "<primitive>"},
                {typeof(CompoundProcedure),  "<procedure>"}
            };

            if (table.ContainsKey(type))
            {
                return table[type];
            }
            else
            {
                throw new ArgumentException($"Unhandled IValue type: {type}");
            }
        }

        public static bool IsIValueType(Type type)
        {
            Type ivt = typeof(IValue);
            return ivt.IsAssignableFrom(type);
        }
    }
}
