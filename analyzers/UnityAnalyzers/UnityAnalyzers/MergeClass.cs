using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace UnityAnalyzers
{
	[Generator]
	public class ExampleSourceGenerator : ISourceGenerator
	{
		
		public void Execute(GeneratorExecutionContext context)
		{
			var writer = new CodeWriter();
			
			var callingEntrypoint = context.Compilation.GetEntryPoint(context.CancellationToken);
			var types = context.Compilation.SourceModule.ReferencedAssemblySymbols;
			
			writer.WriteLine("using System;");
			writer.WriteLine("using UnityEngine;");
			writer.WriteLine("#if UNITY_EDITOR");
			writer.WriteLine("using UnityEditor;");
			writer.WriteLine("#endif");
			writer.WriteLine($"namespace {callingEntrypoint!.ContainingNamespace.ContainingNamespace.Name}.{callingEntrypoint!.ContainingNamespace.Name}");
			writer.BeginBlock();
			
			writer.WriteLine("public class TestComponent : MonoBehaviour");
			writer.BeginBlock();
			
			writer.EndBlock();
			
			writer.EndBlock();
			
			foreach (var type in types)
			{
				foreach (var mod in type.Modules)
				{
					
				}
			}

			context.AddSource("TestComponent", SourceText.From(writer.ToString(), Encoding.UTF8));
		}

		public void Initialize(GeneratorInitializationContext context) { }
	}
}