using System;


namespace Nero.Parsing
{
    static class Analyzer
    {
        /// <summary>
        /// Parses S-Expressions and builds an internal representation. 
        /// Named after the 'analyze procedure from SICP #4.1.7.
        /// </summary>
        /// <param name="sExpr"></param>
        /// <returns></returns>
        public static Expression Analyze(SExpression sExpression)
        {
            // Dispatch on S-Expression's type

            if (sExpression is SAtom atom)
            {
                if (NumberExpression.IsInstance(atom))
                {
                    return NumberExpression.Analyze(atom);
                }
                else if (StringExpression.IsInstance(atom))
                {
                    return StringExpression.Analyze(atom);
                }
                else if (BooleanExpression.IsInstance(atom))
                {
                    return BooleanExpression.Analyze(atom);
                }
                else
                {
                    return Variable.Analyze(atom);
                }
            }
            else
            {
                var list = (SList)sExpression;
                if (list.Count == 0)
                    throw new BadSyntaxException("procedure application", "Empty application", list.Text);

                string head = list[0].Text;

                if (SpecialForm.Table.ContainsKey(head))
                {
                    // $list is a special form

                    Func<SList, SpecialForm> spFormAnalyzer = SpecialForm.Table[head];
                    return spFormAnalyzer(list);
                }
                else
                {
                    // $list is a procedure application

                    return Application.Analyze(list);
                }
            }
        }

    }
}
