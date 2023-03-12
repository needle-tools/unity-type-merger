using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace UnityAnalyzers
{
	[Generator]
	public class ComponentGenerator : ISourceGenerator
	{
		public void Execute(GeneratorExecutionContext context)
		{
			// Doesnt work for some reason
			// var writer = new CodeWriter();
			// var callingEntrypoint = context.Compilation.GetEntryPoint(context.CancellationToken);
			// writer.WriteLine("using System;");
			// writer.WriteLine("using UnityEngine;");
			// writer.WriteLine("#if UNITY_EDITOR");
			// writer.WriteLine("using UnityEditor;");
			// writer.WriteLine("#endif");
			// writer.WriteLine($"namespace {callingEntrypoint!.ContainingNamespace.ContainingNamespace.Name}.{callingEntrypoint!.ContainingNamespace.Name}");
			// writer.BeginBlock();
			// writer.WriteLine("[AddComponentMenu(\"Needle/Test\")]");
			// writer.WriteLine("public class TestComponent : MonoBehaviour");
			// writer.BeginBlock();
			// writer.EndBlock();
			// writer.EndBlock();
			// context.AddSource("TestComponent", SourceText.From(writer.ToString(), Encoding.UTF8));
		}

		public void Initialize(GeneratorInitializationContext context) { }
	}
}