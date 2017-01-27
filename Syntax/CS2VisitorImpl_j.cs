﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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
            var id = context.ID();

            var arguments = new List<object>();

            foreach (var argumentContext in context.argument())
            {
                object value;

                if (argumentContext.ID() != null)
                    value = contextHolder.GetEffective()[argumentContext.ID().GetText()].Value;
                else
                    value = Visit(argumentContext.GetChild(0));

                arguments.Add(value);
            }

            return ExecuteFunction(id.GetText(), arguments.ToArray());
        }

        public object ExecuteFunction<T>(string functionName, params T[] arguments)
        {

            if (functionName.Equals("WriteLine"))
            {
                Console.WriteLine(arguments[0]);
                return null;
            }


            object returnValue;
            var function = (CS2Parser.Function_declarationContext) functionHolder[functionName];
            contextHolder.PushFrame();
            {
                var parameterList = (object[])Visit(function.parameter_list());
                for (var index = 0; index < arguments.Length; index++)
                {
                    contextHolder.GetEffective()[parameterList[index].ToString()].Value = arguments[index];
                }

                returnValue = Visit(function.block());
            }
            contextHolder.PopFrame();
            return returnValue;
        }

        public override object VisitArgument(CS2Parser.ArgumentContext context)
        {
            return Visit(context.GetChild(0));
        }

        /// <summary>
        /// Returns the enum UnaryOperator
        /// </summary>
        public override object VisitPre_unary_operator(CS2Parser.Pre_unary_operatorContext context)
        {
            if (context.GetType() == typeof(RuleContext))
                return Visit(context.post_unary_operator());

            return UnaryOperator.Negate;
        }

        /// <summary>
        /// Returns the enum UnaryOperator
        /// </summary>
        public override object VisitPost_unary_operator(CS2Parser.Post_unary_operatorContext context)
        {
            if (context.OP_DEC() != null)
                return UnaryOperator.Decrement;

            if (context.OP_INC() != null)
                return UnaryOperator.Increment;

            throw new Exception("Post_unary_operator context dosn't contain a post unary operator...");
        }

        public override object VisitType(CS2Parser.TypeContext context)
        {
            if (context.TYPE_DOUBLE() != null)
                return TypeToken.Double;
            if (context.TYPE_INT() != null)
                return TypeToken.Int;
            if (context.TYPE_STRING() != null)
                return TypeToken.String;
            if (context.TYPE_VOID() != null)
                return TypeToken.Void;
            throw new Exception("TypeContext context dosn't contain a type token...");

        }

        public override object VisitArrayType(CS2Parser.ArrayTypeContext context)
        {
            if (context.TYPE_DOUBLE() != null)
                return TypeToken.DoubleArray;
            if (context.TYPE_INT() != null)
                return TypeToken.IntArray;
            if (context.TYPE_STRING() != null)
                return TypeToken.StringArray;
            throw new Exception("ArrayTypeContext context dosn't contain an array type token...");

        }
    }

    internal enum TypeToken
    {
        Double,
        Void,
        String,
        Int,
        StringArray,
        IntArray,
        DoubleArray
    }

    internal enum UnaryOperator
    {
        Increment,
        Decrement,
        Negate,
    }
}
