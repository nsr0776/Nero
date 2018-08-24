using System;
using System.Collections.Generic;
using System.Text;


namespace Nero.Parsing
{
    static class Scanner
    {
        class PushdownAutomaton
        {
            public enum State { Listening, Symbol, String, Comment }

            public PushdownAutomaton()
            {
                CurrentState = State.Listening;
                stack.Push(new SList());
            }

            public State CurrentState { get; set; }

            private Stack<SList> stack = new Stack<SList>();

            private StringBuilder cache = new StringBuilder();

            public void Push()
            {
                stack.Push(new SList());
            }

            public void PopAndCheck()
            {
                if (stack.Count == 1)
                    throw new ScannerErrorException("No matching opening bracket");

                SList topList = stack.Pop();
                //if (topList.Count == 0)
                //    throw new ScannerException("Empty application");

                stack.Peek().Add(topList);
            }

            public void AddAndClearCache()
            {
                var symbol = new SAtom(cache.ToString());
                stack.Peek().Add(symbol);
                cache.Clear();
            }

            public void AppendCharToCache(char ch)
            {
                cache.Append(ch);
            }

            public IReadOnlyList<SExpression> CheckAndReturn()
            {
                if (stack.Count > 1)
                    throw new ScannerErrorException($"Unclosed opening brackets");

                return stack.Peek();
            }
        }

        /// <summary>
        /// Converts source code string into well formed S-Expressions.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IReadOnlyList<SExpression> Read(IEnumerable<char> input)
        {
            PushdownAutomaton pda = new PushdownAutomaton();

            // Process input char stream
            foreach (var ch in input)
            {
                switch (pda.CurrentState)
                {
                    case PushdownAutomaton.State.Listening:
                        if (Utils.IsLeftBracket(ch))
                        {
                            pda.Push();
                        }
                        else if (Utils.IsRightBracket(ch))
                        {
                            pda.PopAndCheck();
                        }
                        else if (char.IsWhiteSpace(ch))
                        {
                            // do nothing
                        }
                        else if (ch == ';')
                        {
                            pda.CurrentState = PushdownAutomaton.State.Comment;
                        }
                        else if (ch == '\"')
                        {
                            pda.AppendCharToCache(ch);
                            pda.CurrentState = PushdownAutomaton.State.String;
                        }
                        else
                        {
                            pda.AppendCharToCache(ch);
                            pda.CurrentState = PushdownAutomaton.State.Symbol;
                        }
                        break;
                    case PushdownAutomaton.State.Symbol:
                        if (Utils.IsLeftBracket(ch))
                        {
                            pda.AddAndClearCache();
                            pda.Push();
                            pda.CurrentState = PushdownAutomaton.State.Listening;
                        }
                        else if (Utils.IsRightBracket(ch))
                        {
                            pda.AddAndClearCache();
                            pda.PopAndCheck();
                            pda.CurrentState = PushdownAutomaton.State.Listening;
                        }
                        else if (char.IsWhiteSpace(ch))
                        {
                            pda.AddAndClearCache();
                            pda.CurrentState = PushdownAutomaton.State.Listening;
                        }
                        else if (ch == ';')
                        {
                            pda.AddAndClearCache();
                            pda.CurrentState = PushdownAutomaton.State.Comment;
                        }
                        else if (ch == '\"')
                        {
                            pda.AddAndClearCache();
                            pda.AppendCharToCache(ch);
                            pda.CurrentState = PushdownAutomaton.State.String;
                        }
                        else
                        {
                            pda.AppendCharToCache(ch);
                        }
                        break;
                    case PushdownAutomaton.State.String:
                        if (ch == '\"')
                        {
                            pda.AppendCharToCache(ch);
                            pda.AddAndClearCache();
                            pda.CurrentState = PushdownAutomaton.State.Listening;
                        }
                        else
                        {
                            pda.AppendCharToCache(ch);
                        }
                        break;
                    case PushdownAutomaton.State.Comment:
                        if (ch == '\n')
                        {
                            pda.CurrentState = PushdownAutomaton.State.Listening;
                        }
                        break;
                    default:
                        throw new ArgumentException($"Unknown state: {pda.CurrentState}");
                }
            }


            // Process the EOF
            if (pda.CurrentState == PushdownAutomaton.State.Symbol)
                pda.AddAndClearCache();
            else if (pda.CurrentState == PushdownAutomaton.State.String)
                throw new ScannerErrorException("Unclosed string");


            return pda.CheckAndReturn();
        }
    }
}
