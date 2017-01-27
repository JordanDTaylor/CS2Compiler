using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;

namespace Syntax
{
    partial class CS2VisitorImpl : CS2BaseVisitor<Object>
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
        public override Object VisitRelational_operation([NotNull] CS2Parser.Relational_operationContext context)
        {
            Boolean ret;
            Relop op = (Relop)Visit(context.children[1]);
            Object left = Visit(context.children[0]);
            Object right = Visit(context.children[2]);
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
                    throw new Exception("Unsupported relational operator: "+op);
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
        public override Object VisitRelop([NotNull] CS2Parser.RelopContext context)
        {
            switch (context.children[0].GetText())
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
                    throw new Exception("Unsupported relational operator: " + context.children[0].GetText());
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
        public override Object VisitExpression([NotNull] CS2Parser.ExpressionContext context)
        {
            Object result = Visit(context.children[0]);
            for(int i=2; i<context.children.Count; i+=2)
            {
                string op = context.children[i - 1].GetText();
                Object right = Visit(context.children[i]);
                switch (op)
                {
                    case "+":
                        if (result.GetType() == typeof(string))
                        {
                            result += (string)right;
                        }
                        else
                        {
                            result = ((Double)result) + (Double)right;
                        }
                        break;
                    case "-":
                        if (result.GetType() == typeof(string))
                        {
                            result += (string)right;
                        }
                        else
                        {
                            result = ((Double)result) + (Double)right;
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
        public override Object VisitMultiplyingExpression([NotNull] CS2Parser.MultiplyingExpressionContext context)
        {
            Double result = (Double)Visit(context.children[0]);
            for(int i=2; i<context.children.Count; i += 2)
            {
                string op = context.children[i-1].GetText();
                Double right = (Double)Visit(context.children[i]);
                switch (op)
                {
                    case "*":
                        result *= right;
                        break;
                    case "/":
                        result /= right;
                        break;
                    default:
                        throw new Exception("Unsupported MULT/DIV operator: "+op);
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
        public override Object VisitAtom([NotNull] CS2Parser.AtomContext context)
        {
            if (context.children.Count > 1)
            {
                return Visit(context.children[1]);
            }
            double d;
            if (double.TryParse(context.children[0].GetText(), out d))
            {
                return d;
            }
            if (context.children[0].GetType() == typeof(CS2Parser.Function_callContext))
            {
                return Visit(context.children[0]);
            }
            return contextHolder.GetEffective()[context.children[0].GetText()].Value;
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
        public override Object VisitConstant([NotNull] CS2Parser.ConstantContext context)
        {
            if (context.children[0].GetType() == typeof(CS2Parser.Char_constantContext) || context.children[0].GetType()==typeof(CS2Parser.String_constantContext))
            {
                return Visit(context.children[0]);
            }
            return Double.Parse(context.children[0].GetText());
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
        public override Object VisitChar_constant([NotNull] CS2Parser.Char_constantContext context)
        {
            return context.children[1].GetText()[0];
        }

    }
}
