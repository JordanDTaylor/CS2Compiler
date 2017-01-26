using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax
{
    partial class EvalVisitor : CS2BaseVisitor<System.Object>
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
        public override System.Object VisitRelational_operation([NotNull] CS2Parser.Relational_operationContext context)
        {
            Boolean ret;
            Relop op = (Relop)Visit(context.children[1]);
            System.Object left = Visit(context.children[0]);
            System.Object right = Visit(context.children[2]);
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
        public override System.Object VisitRelop([NotNull] CS2Parser.RelopContext context)
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
        public override System.Object VisitExpression([NotNull] CS2Parser.ExpressionContext context)
        {
            //TODO: 
            return VisitChildren(context);
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
        public override System.Object VisitMultiplyingExpression([NotNull] CS2Parser.MultiplyingExpressionContext context)
        {
            //TODO: 
            return VisitChildren(context);
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
        public override System.Object VisitAtom([NotNull] CS2Parser.AtomContext context)
        {
            //TODO: 
            return VisitChildren(context);
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
        public override System.Object VisitConstant([NotNull] CS2Parser.ConstantContext context)
        {
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
        public override System.Object VisitChar_constant([NotNull] CS2Parser.Char_constantContext context)
        {
            return context.children[0].GetText()[0];
        }

    }
}
