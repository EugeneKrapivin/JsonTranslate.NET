//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\eugene.krapivin@sap.com\source\repos\TranformerDSLParser\TranformerDSLParser\TransformerDSL.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace TransformerDSL.Parser {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="TransformerDSLParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.CLSCompliant(false)]
public interface ITransformerDSLListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransformerDSLParser.func"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunc([NotNull] TransformerDSLParser.FuncContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransformerDSLParser.func"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunc([NotNull] TransformerDSLParser.FuncContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransformerDSLParser.json"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterJson([NotNull] TransformerDSLParser.JsonContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransformerDSLParser.json"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitJson([NotNull] TransformerDSLParser.JsonContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransformerDSLParser.obj"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterObj([NotNull] TransformerDSLParser.ObjContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransformerDSLParser.obj"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitObj([NotNull] TransformerDSLParser.ObjContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransformerDSLParser.pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPair([NotNull] TransformerDSLParser.PairContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransformerDSLParser.pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPair([NotNull] TransformerDSLParser.PairContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransformerDSLParser.arr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArr([NotNull] TransformerDSLParser.ArrContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransformerDSLParser.arr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArr([NotNull] TransformerDSLParser.ArrContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransformerDSLParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterValue([NotNull] TransformerDSLParser.ValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransformerDSLParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitValue([NotNull] TransformerDSLParser.ValueContext context);
}
} // namespace TransformerDSL.Parser
