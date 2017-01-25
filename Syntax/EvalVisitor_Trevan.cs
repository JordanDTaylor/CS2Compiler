using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax
{
    partial class EvalVisitor : CS2BaseVisitor<Object>
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
            return VisitChildren(context);
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
        public override Object VisitMultiplyingExpression([NotNull] CS2Parser.MultiplyingExpressionContext context)
        {
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
        public override Object VisitAtom([NotNull] CS2Parser.AtomContext context)
        {
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
        public override Object VisitConstant([NotNull] CS2Parser.ConstantContext context)
        {
            return VisitChildren(context);
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
            return VisitChildren(context);
        }

    }
}
