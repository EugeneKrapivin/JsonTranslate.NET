using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using JsonTranslate.NET.Core.Abstractions;

namespace JsonTranslate.NET.Core.JustDsl
{
    public class JustDslSerializer : ISerializeDsl
    {
        public string Serialize(Instruction instruction)
        {
            var sb = new StringBuilder();

            sb.Append($"#{instruction.Name}(");

            if (instruction.Config != null)
            {
                sb.Append(instruction.Config.ToString(Newtonsoft.Json.Formatting.None));
            }

            if (instruction.Bindings?.Any() == true)
            {
                if (instruction.Config != null) sb.Append(", ");
#if NETSTANDARD2_1
                sb.AppendJoin(", ", instruction.Bindings.Select(Serialize));
#else
                sb.Append(string.Join(", ", instruction.Bindings.Select(Serialize)));
#endif

            }

            sb.Append(")");

            return sb.ToString();
        }

        public Instruction Deserialize(string dsl)
        {
            if (dsl == null) throw new ArgumentNullException(nameof(dsl));

            var antlrStream = new AntlrInputStream(dsl);
            // TODO: add error listener to spot lexing errors
            var lexer = new JustDslLexer(antlrStream);
            var tokenizer = new CommonTokenStream(lexer);
            var ctx = GetParser(tokenizer);
            var visitor = new DslVisitor();

            return visitor.Visit(ctx);
        }

        private static JustDslParser.StartContext GetParser(CommonTokenStream tokenizer)
        {
            if (tokenizer == null) throw new ArgumentNullException(nameof(tokenizer));

            var parser = new JustDslParser(tokenizer);

            //var parserErrorListener = new ErrorListener<IToken>();
           // parser.AddErrorListener(parserErrorListener);

            var ctx = parser.start();
            //if (parserErrorListener.Faulted)
            //{
            //    // TODO: log
            //    // TODO: domain exception
            //    throw new Exception(@"Encountered errors during parsing");
            //}

            return ctx;
        }
    }

    //public class ErrorListener<S> : ConsoleErrorListener<S>
    //{
    //    public bool Faulted;

    //    public override void SyntaxError(TextWriter output, IRecognizer recognizer, S offendingSymbol, int line,
    //        int col, string msg, RecognitionException e)
    //    {
    //        Faulted = true;
    //        base.SyntaxError(output, recognizer, offendingSymbol, line, col, msg, e);
    //    }
    //}
}
