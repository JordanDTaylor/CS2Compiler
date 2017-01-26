namespace Syntax
{
    // ReSharper disable once InconsistentNaming
	partial class CS2VisitorImpl
    {
        public override object VisitString_constant(CS2Parser.String_constantContext context)
        {
            return context.GetText();
        }

        public override object VisitFunction_call(CS2Parser.Function_callContext context)
        {
            return null;
        }

        public override object VisitArgument(CS2Parser.ArgumentContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override object VisitPre_unary_operator(CS2Parser.Pre_unary_operatorContext context)
        {
            return base.VisitPre_unary_operator(context);
        }

        public override object VisitPost_unary_operator(CS2Parser.Post_unary_operatorContext context)
        {
            return base.VisitPost_unary_operator(context);
        }

        public override object VisitType(CS2Parser.TypeContext context)
        {
            return context.GetText();
        }

        public override object VisitArrayType(CS2Parser.ArrayTypeContext context)
        {
            return base.VisitArrayType(context);
        }
    }

    internal enum UnaryOperator
    {
        Increment,
        Decrement,
        Negate,
    }
}
