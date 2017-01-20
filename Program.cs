using System;
using System.IO;
using Antlr4.Runtime;

namespace CS2Compiler
{
    class Program
    {
        private static void Main(string[] args)
        {
            (new Program()).Run();
        }
        public void Run()
        {
            try
            {
                Console.WriteLine("START");
                RunParser();
                Console.Write("DONE. Hit RETURN to exit: ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
                Console.Write("Hit RETURN to exit: ");
            }
            Console.ReadLine();
        }
        private void RunParser()
        {
            AntlrInputStream inputStream = new AntlrInputStream(File.ReadAllText("sqrt.cs2"));
            CS2Lexer cs2Lexer = new CS2Lexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(cs2Lexer);
            CS2Parser cs2Parser = new CS2Parser(commonTokenStream);
            CS2Parser.ArgumentContext rContext = cs2Parser.argument();
            MyVisitor visitor = new MyVisitor();
            visitor.Visit(rContext);
        }
    }

    internal class MyVisitor {
        public void Visit(CS2Parser.ArgumentContext rContext)
        {
            Console.WriteLine(rContext);
        }
    }
}
