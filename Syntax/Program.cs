using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using static CS2Parser;

namespace Syntax
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().RunParser();
		}

		private void RunParser()
		{
			AntlrInputStream inputStream = new AntlrInputStream(File.ReadAllText("sqrt.cs2"));
			CS2Lexer cs2Lexer = new CS2Lexer(inputStream);

			CommonTokenStream commonTokenStream = new CommonTokenStream(cs2Lexer);
			CS2Parser cs2Parser = new CS2Parser(commonTokenStream);
			ProgramContext pContext = cs2Parser.program();

			Enumerate(pContext, 0, new Context<string, TypedVariable>());
		}

		private void Enumerate(ITree root, int indent, Context<string, TypedVariable> context)
		{
			Console.WriteLine(new string('-', indent) +
				root.Payload.GetType() + " " + (root.Payload is IToken ? ((IToken)root.Payload).Text : root.Payload));

			if (root.GetType() == typeof(Function_declarationContext) ||
				root.GetType() == typeof(BlockContext))
			{
				context.PushFrame();
			}

			if (root.GetType() == typeof(ParameterContext))
				HandleParameterContext((ParameterContext)root, context);
			else if (root.GetType() == typeof(DeclarationContext))
			{
				if (!HandleDeclarationContext((DeclarationContext)root, context))
					Console.WriteLine("Duplicate declaration");
			}

			for (int i = 0; i < root.ChildCount; i++)
			{
				ITree child = root.GetChild(i);
				Enumerate(child, indent + 1, context);
			}

			// Run this after the enumeration so that declarations are proccessed
			// before assignments
			if (root.GetType() == typeof(AssignmentContext))
			{
				if (!EvaluateAssignmentContext((AssignmentContext)root, context))
					Console.WriteLine("Invalid assignment");
			}

			if (root.GetType() == typeof(Function_declarationContext) ||
				root.GetType() == typeof(BlockContext))
			{
				context.PopFrame();
			}
		}

		private void HandleParameterContext(ParameterContext value, Context<string, TypedVariable> context)
		{
			string type = GetType((TypeContext)value.GetChild(0));

			CommonToken name = (CommonToken)value.GetChild(1).Payload;

			context.AddToCurrent(name.Text,
				new TypedVariable()
				{
					Type = type,
					Name = name.Text
				});
		}

		private bool HandleDeclarationContext(DeclarationContext value, Context<string, TypedVariable> context)
		{
			bool error = false;

			DeclarationContext(value, (type, name) =>
			{
				// This is not exactly C#'s behavior, but....
				if (context.GetEffective().ContainsKey(name))
					error = true;

				context.AddToCurrent(name,
					new TypedVariable()
					{
						Type = type,
						Name = name
					});
			});

			return !error;
		}

		private bool EvaluateAssignmentContext(AssignmentContext value, Context<string, TypedVariable> context)
		{
			List<string> tokens = new List<string>();

			if (value.GetChild(0).Payload is CommonToken)
				tokens.Add(((CommonToken)value.GetChild(0).Payload).Text);
			else
			{
				DeclarationContext declaration = (DeclarationContext)value.GetChild(0).Payload;
				DeclarationContext(declaration, (type, name) => tokens.Add(name));
			}

			EvaluatableContext evaluatable = ((EvaluatableContext)value.GetChild(2).Payload);

			Dictionary<string, TypedVariable> effective = context.GetEffective();

			foreach (string token in tokens)
			{
				if (!effective.ContainsKey(token))
					return false;

				TypedVariable variable = effective[token];

				// TODO: How do we know what type we want?
				Validate(evaluatable, (type) => { return true; },
					(constant) => { return true; }, context);
			}

			return true;
		}

		private string GetType(TypeContext typeContext)
		{
			string type = null;

			if (typeContext.GetChild(0).Payload is CommonToken)
				type = ((CommonToken)typeContext.GetChild(0).Payload).Text;
			else
			{
				ArrayTypeContext arrayType = (ArrayTypeContext)typeContext.GetChild(0);
				type = ((CommonToken)arrayType.GetChild(0).Payload).Text + "[]";
			}

			return type;
		}

		private void DeclarationContext(DeclarationContext value, Action<string, string> action)
		{
			string type = GetType((TypeContext)value.GetChild(0));

			for (int i = 1; i < value.ChildCount; i++)
			{
				CommonToken name = (CommonToken)value.GetChild(i).Payload;

				if (name.Text != ",")
					action(type, name.Text);
			}
		}

		private bool Validate(EvaluatableContext evaluatable, Func<string, bool> typeValidator, Func<string, bool> constantValidator,
			Context<string, TypedVariable> context)
		{
			return ValidateHelper(evaluatable, typeValidator, constantValidator, context);
		}

		private bool ValidateHelper(ITree step, Func<string, bool> typeValidator, Func<string, bool> constantValidator,
			Context<string, TypedVariable> context)
		{
			bool enumerateChildren = true;

			if (step.Payload.GetType() == typeof(CommonToken))
			{
				if (step.Parent.ChildCount == 1)
				{
					if (step.Parent.GetType() == typeof(AtomContext))
					{
						// variable or constant
						string value = ((CommonToken)step.Payload).Text;

						double parsed = 0.0;

						if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
							double.TryParse(value, out parsed))
						{
							enumerateChildren = false;

							if (!constantValidator(value))
								return false;
						}
						else
						{
							enumerateChildren = false;

							if (!typeValidator(context.GetEffective()[value].Type))
								return false;
						}
					}
					else if (step.Parent.GetType() == typeof(EvaluatableContext))
					{
						enumerateChildren = false;

						// variable
						if (!typeValidator(context.GetEffective()[((CommonToken)step.Payload).Text].Type))
							return false;
					}
				}
			}
			else if (step.Payload.GetType() == typeof(ConstantContext))
			{
				// constant
				enumerateChildren = false;

				if (!constantValidator(((CommonToken)step.GetChild(0).Payload).Text))
					return false;
			}

			if (enumerateChildren)
			{
				for (int i = 0; i < step.ChildCount; i++)
					if (!ValidateHelper(step.GetChild(i), typeValidator, constantValidator, context))
						return false;
			}

			return true;
		}

		/*
		 * CS2Parser+ParameterContext
		 * CS2Parser+DeclarationContext
		 * CS2Parser+AssignmentContext
		 * 
		 * CS2Parser+BlockContext
		 * CS2Parser+Function_declarationContext
		 */


		/*
		 * ConstantContext
		 * AtomContext
		 * CommonToken
		 */
	}
}
