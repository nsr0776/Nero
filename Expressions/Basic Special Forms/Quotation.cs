using System;


namespace Nero
{
    /// <summary>
    /// Models the "quote" expression in the target language.
    /// A basic special form.
    /// </summary>
    class Quotation : SpecialForm
    {
        public Quotation(string quotationText)
        {
            QuotationText = quotationText;
        }

        public string QuotationText { get; private set; }

        public static string KeyWord { get; } = "quote";

        public override IValue Evaluate(Environment env)
        {
            return new Symbol(QuotationText);
        }

        public static Quotation Analyze(Parsing.SList expr)
        {
            if (expr.Count != 2)
                throw new BadSyntaxException(KeyWord, string.Empty, expr.Text);

            string text = expr[1].Text;
            return new Quotation(text);
        }
    }
}
