using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax
{
    partial class CS2VisitorImpl : CS2BaseVisitor<object>
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
        public override object VisitFor_loop([NotNull] CS2Parser.For_loopContext context)
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
        public override object VisitWhile_loop([NotNull] CS2Parser.While_loopContext context)
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
        public override object VisitParameter_list([NotNull] CS2Parser.Parameter_listContext context)
        {
            int childCount = context.ChildCount;
            var paramaters = new object[childCount];

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
        public override object VisitParameter([NotNull] CS2Parser.ParameterContext context)
        {
            var id = context.children[1].GetText();
            var typedVar = new TypedVariable
            {
                Name = id,
                Type = context.children[0].GetText()
            };
            contextHolder.AddToCurrent(id, typedVar);
            return id;
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
        public override object VisitAssignment([NotNull] CS2Parser.AssignmentContext context)
        {
            // (declaration | ID) '=' evaluatable ';'
            string name;
            var firstChild = context.children[0];
            if (firstChild.GetType() == typeof(CS2Parser.DeclarationContext))
                name = Visit((CS2Parser.DeclarationContext)firstChild).ToString();
            else
                name = firstChild.GetText();
            contextHolder.GetEffective()[name].Value = Visit(context.children[2]);

            return null;
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
        public override object VisitEvaluatable([NotNull] CS2Parser.EvaluatableContext context)
        {
            var id = context.ID();
            if (id == null)
                return Visit(context.children[0]);
            else
                return contextHolder.GetEffective()[id.GetText()].Value;
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
        public override object VisitOperation([NotNull] CS2Parser.OperationContext context)
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
        public override object VisitUnary_operation([NotNull] CS2Parser.Unary_operationContext context)
        { // TODO value needs to be updated in the dictionary.
            /*
                unary_operation
                    : (pre_unary_operator expression)
            | (expression post_unary_operator)
            ;
            */
            IParseTree child0 = context.children[0];
            IParseTree child1 = context.children[1];
            double value;
            UnaryOperator opp;


            var id = context.expression().GetText();

            if (child0.GetType() == typeof(CS2Parser.Pre_unary_operatorContext)) // pre_unary_operator expression
            {
                opp = (UnaryOperator) Visit(context.pre_unary_operator()); 
                value = (double)Visit(child1);
            }
            else // expression post_unary_operator
            {
                value = (double)Visit(child0);
                opp = (UnaryOperator)Visit(context.post_unary_operator());
            }

            switch (opp)
            {
                case UnaryOperator.Increment:
                    value++;
                    break;
                case UnaryOperator.Decrement:
                    value--;
                    break;
                case UnaryOperator.Negate:
                    value = -value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            contextHolder.GetEffective()[id].Value = value;

            return value;
        }
    }
}
