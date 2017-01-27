using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using static CS2Parser;

namespace Syntax
{
	partial class CS2VisitorImpl : CS2BaseVisitor<object>
	{
		private Context<string, TypedVariable> contextHolder = new Context<string, TypedVariable>();
		private Dictionary<string, IParseTree> functionHolder = new Dictionary<string, IParseTree>();

		public override object VisitProgram([NotNull] ProgramContext context)
		{
			for (int i = 0; i < context.ChildCount; i++)
				Visit(context.GetChild(i));

			// TODO: Call functionHolder["Main"]

			return default(object);
		}

		public override object VisitDeclaration([NotNull] DeclarationContext context)
		{
			string type = VisitType((TypeContext)context.GetChild(0)).ToString();
			string name = null;

			for (int i = 1; i < context.ChildCount; i++)
			{
				name = context.GetChild(i).ToString();

				if (name != ",")
				{
					contextHolder.AddToCurrent(name,
						new TypedVariable() { Type = type, Name = name });
				}
			}

			return name;
		}

		public override object VisitFunction_declaration([NotNull] Function_declarationContext context)
		{
			int index = 0;

			if (context.GetChild(index) is ModContext)
			{
				VisitMod((ModContext)context.GetChild(index));
				index++;
			}

			string type = VisitType((TypeContext)context.GetChild(index++)).ToString();
			string name = context.GetChild(index++).ToString();

			functionHolder.Add(name, context);

			return default(object);
		}

		public override object VisitMod([NotNull] ModContext context)
		{
			return context.GetChild(0).ToString();
		}

		public override object VisitStatement([NotNull] StatementContext context)
		{
			RuleContext child0 = (RuleContext)context.GetChild(0);
			Type child0Type = child0.GetType();

			if (child0Type == typeof(For_loopContext))
				return VisitFor_loop((For_loopContext)child0);
			else if (child0Type == typeof(While_loopContext))
				return VisitWhile_loop((While_loopContext)child0);
			else if (child0Type == typeof(Function_callContext))
				return VisitFunction_call((Function_callContext)child0);
			else if (child0Type == typeof(DeclarationContext))
				return VisitDeclaration((DeclarationContext)child0);
			else if (child0Type == typeof(AssignmentContext))
				return VisitAssignment((AssignmentContext)child0);
			else if (child0Type == typeof(BlockContext))
				return VisitBlock((BlockContext)child0);
			else if (child0Type == typeof(If_statementContext))
				return VisitIf_statement((If_statementContext)child0);
			else if (child0Type == typeof(Return_statementContext))
				return VisitReturn_statement((Return_statementContext)child0);

			throw new InvalidOperationException();
		}

		public override object VisitReturn_statement([NotNull] Return_statementContext context)
		{
			RuleContext child1 = (RuleContext)context.GetChild(1);

			if (child1 is EvaluatableContext)
				return VisitEvaluatable((EvaluatableContext)child1);

			return default(object);
		}

		public override object VisitBlock([NotNull] BlockContext context)
		{
			contextHolder.PushFrame();

			object result = default(object);

			// Skip the open and closing braces
			for (int i = 1; i < context.ChildCount - 1; i++)
			{
				result = VisitStatement((StatementContext)context.GetChild(i));

				if (context.GetChild(i) is Return_statementContext)
					break;
			}

			contextHolder.PopFrame();

			return result;
		}

		public override object VisitIf_statement([NotNull] If_statementContext context)
		{
			EvaluatableContext evaluatable = (EvaluatableContext)context.GetChild(2);

			if (VisitEvaluatable(evaluatable).Equals(true))
				return VisitBlock((BlockContext)context.GetChild(4));
			else
			{
				if (context.ChildCount > 6)
					return VisitBlock((BlockContext)context.GetChild(6));
			}

			return default(object);
		}
	}
}