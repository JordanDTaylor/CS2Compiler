using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Syntax
{
    partial class CS2VisitorAsm : CS2BaseVisitor<object>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.relational_operation"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override object VisitRelational_operation([NotNull] CS2Parser.Relational_operationContext context)
        {
            bool ret;
            Relop op = (Relop)Visit(context.GetChild(1));
            object left = Visit(context.GetChild(0));
            object right = Visit(context.GetChild(2));
            switch (op)
            {
                case Relop.EQ:
                    ret = left.Equals(right);
                    break;
                case Relop.LT:
                    ret = ((IComparable)left).CompareTo(right) < 0;
                    break;
                case Relop.GT:
                    ret = ((IComparable)left).CompareTo(right) > 0;
                    break;
                case Relop.LE:
                    ret = ((IComparable)left).CompareTo(right) <= 0;
                    break;
                case Relop.GE:
                    ret = ((IComparable)left).CompareTo(right) >= 0;
                    break;
                default:
                    throw new Exception("Unsupported relational operator: " + op);
            }
            return ret;
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.relop"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override object VisitRelop([NotNull] CS2Parser.RelopContext context)
        {
            switch (context.GetChild(0).GetText())
            {
                case "==":
                    return Relop.EQ;
                case "<":
                    return Relop.LT;
                case ">":
                    return Relop.GT;
                case "<=":
                    return Relop.LE;
                case ">=":
                    return Relop.GE;
                default:
                    throw new Exception("Unsupported relational operator: " + context.GetChild(0).GetText());
            }
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.expression"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override object VisitExpression([NotNull] CS2Parser.ExpressionContext context)
        {
            object result = Visit(context.GetChild(0));
            for (int i = 2; i < context.children.Count; i += 2)
            {
                string op = context.GetChild(i - 1).GetText();
                object right = Visit(context.GetChild(i));

                switch (op)
                {
                    case "+":
                        if (result is string)
                        {
                            result += (string)right;
                        }
                        else
                        {
                            result = (double)result + (double)right;
                        }
                        break;

                    case "-":
                        if (result is string)
                        {
                            result += (string)right;
                        }
                        else
                        {
                            result = (double)result - (double)right;
                        }
                        break;
                    default:
                        throw new Exception("Unsupported ADD/SUB operator: " + op);
                }
            }
            return result;
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.multiplyingExpression"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override object VisitMultiplyingExpression([NotNull] CS2Parser.MultiplyingExpressionContext context)
        {
            double result = (double)Visit(context.GetChild(0));
            for (int i = 2; i < context.children.Count; i += 2)
            {
                string op = context.GetChild(i - 1).GetText();
                double right = (double)Visit(context.GetChild(i));
                switch (op)
                {
                    case "*":
                        result *= right;
                        break;
                    case "/":
                        result /= right;
                        break;
                    default:
                        throw new Exception("Unsupported MULT/DIV operator: " + op);
                }
            }
            return result;
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.atom"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override object VisitAtom([NotNull] CS2Parser.AtomContext context)
        {
            if (context.children.Count > 1)
            {
                return Visit(context.GetChild(1));
            }
            double d;
            if (double.TryParse(context.GetChild(0).GetText(), out d))
            {
                return d;
            }
            if (context.GetChild(0).GetType() == typeof(CS2Parser.Function_callContext))
            {
                return Visit(context.GetChild(0));
            }
            return contextHolder.GetEffective()[context.GetChild(0).GetText()].Value;
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.constant"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override object VisitConstant([NotNull] CS2Parser.ConstantContext context)
        {
            return context.GetChild(0).GetType() == typeof(RuleContext) ? Visit(context.GetChild(0)) : double.Parse(context.REAL().GetText());
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.char_constant"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override object VisitChar_constant([NotNull] CS2Parser.Char_constantContext context)
        {
            return context.LETTER().GetText()[0];
        }

    }
}
