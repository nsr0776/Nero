using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Nero.Parsing
{
    /// <summary>
    /// Models the S-Expression lists. Contains S-Expressions.
    /// [Invariant] It is guaranteed to be non-empty.
    /// </summary>
    class SList : SExpression, IReadOnlyList<SExpression>, ICollection<SExpression>
    {
        public SList()
        {
            list = new List<SExpression>();
        }

        private List<SExpression> list;

        /// <summary>
        /// Text representation of the S-List.
        /// </summary>
        public override string Text
        {
            get
            {
                if (list.Count == 0)
                    return "()";

                var builder = new StringBuilder();

                builder.Append('(');
                builder.Append(list.First().Text);  // list should contain at least 1 element
                foreach (var sExpr in list.Skip(1))
                {
                    builder.Append(' ');
                    builder.Append(sExpr.Text);
                }
                builder.Append(')');

                return builder.ToString();
            }
        }

        /// <summary>
        /// The number of elements in the S-List.
        /// </summary>
        public int Count => list.Count;

        bool ICollection<SExpression>.IsReadOnly => false;

        public SExpression this[int index] => list[index];

        public void Add(SExpression item)
        {
            list.Add(item);
        }

        public void AddRange(IEnumerable<SExpression> sExprs)
        {
            list.AddRange(sExprs);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(SExpression item)
        {
            return list.Contains(item);
        }

        public void CopyTo(SExpression[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(SExpression item)
        {
            return list.Remove(item);
        }

        public IEnumerator<SExpression> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
