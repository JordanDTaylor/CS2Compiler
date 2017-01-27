using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax
{
    partial class CS2VisitorImpl : CS2BaseVisitor<Object>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.for_loop"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitFor_loop([NotNull] CS2Parser.For_loopContext context)
        {
            Visit(context.children[2]);
            for (; (bool)Visit(context.children[3]); Visit(context.children[5]))
                Visit(context.children[7]);
            return null;
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.while_loop"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitWhile_loop([NotNull] CS2Parser.While_loopContext context)
        {
            while ((bool)Visit(context.children[2]))
                Visit(context.children[4]);
            return null;
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.parameter_list"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitParameter_list([NotNull] CS2Parser.Parameter_listContext context)
        {
            int childCount = context.children.Count;
            Object[] paramaters = new Object[childCount];

            for(int i=0; i<childCount; i++)
            {
                paramaters[i] = Visit(context.children[i]);
            }

            return paramaters;
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.parameter"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitParameter([NotNull] CS2Parser.ParameterContext context)
        {

        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.assignment"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitAssignment([NotNull] CS2Parser.AssignmentContext context)
        {

        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.evaluatable"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitEvaluatable([NotNull] CS2Parser.EvaluatableContext context)
        {

        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.operation"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitOperation([NotNull] CS2Parser.OperationContext context)
        {
            return Visit(context.children[0]);
        }
        /// <summary>
        /// Visit a parse tree produced by <see cref="CS2Parser.unary_operation"/>.
        /// <para>
        /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Object}.VisitChildren(IRuleNode)"/>
        /// on <paramref name="context"/>.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        public override Object VisitUnary_operation([NotNull] CS2Parser.Unary_operationContext context)
        { // TODO value needs to be updated in the dictionary.
            /*
                unary_operation
                    : (pre_unary_operator expression)
            | (expression post_unary_operator)
            ;
            */
            IParseTree child0 = context.children[0];
            IParseTree child1 = context.children[1];
            double expression;
            String opp;
            if(child0.GetType() == typeof(CS2Parser.Pre_unary_operatorContext)) // pre_unary_operator expression
            {
                opp = child0.GetText();
                expression = (double)Visit(child1);
            }
            else // expression post_unary_operator
            {
                expression = (double)Visit(child0);
                opp = child1.GetText();
            }


            if (opp.Equals("++"))
                expression++;
            else if (opp.Equals("--"))
                expression--;
            else if (opp.Equals("-"))
                expression *= -1;
            return expression;
        }
    }
}
