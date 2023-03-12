using System.Globalization;
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
			System.Console.WriteLine(System.DateTime.Now.ToString(CultureInfo.InvariantCulture));
			
			var callingEntrypoint = context.Compilation.GetEntryPoint(context.CancellationToken);
			
			var writer = new CodeWriter();
			writer.WriteLine("using System;");
			writer.WriteLine("using UnityEngine;");
			writer.WriteLine("#if UNITY_EDITOR");
			writer.WriteLine("using UnityEditor;");
			writer.WriteLine("#endif");
			writer.WriteLine($"namespace MyNamespace");// {callingEntrypoint!.ContainingNamespace.ContainingNamespace.Name}.{callingEntrypoint!.ContainingNamespace.Name}");
			writer.BeginBlock();
			writer.WriteLine("public partial class TestComponent : MonoBehaviour");
			writer.BeginBlock();
			writer.WriteLine("public string mySourceGen = \"Hi From Sourcegen\";");
			writer.WriteLine("public void OnEnable()");
			writer.BeginBlock();
			writer.WriteLine("Debug.Log(\"OnEnable\");");
			writer.EndBlock();
			writer.EndBlock();
			writer.EndBlock();
			context.AddSource("TestComponent", SourceText.From(writer.ToString(), Encoding.UTF8));
		}

		public void Initialize(GeneratorInitializationContext context) { }
	}
}