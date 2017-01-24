using System.Runtime.CompilerServices;
using Antlr4.Runtime.Tree;

namespace CS2Compiler
{
    public class EvaluatorWithProps : CS2BaseListener
    {
        readonly ParseTreeProperty<int> values = new ParseTreeProperty<int>(); //node to final value map

        public int this[IParseTree node]
        {
            get { return values.Get(node); }
            set { values.Put(node, value); }
        }
    }
}