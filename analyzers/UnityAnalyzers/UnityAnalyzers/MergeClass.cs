using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace UnityAnalyzers
{
	[Generator]
	public class MergedClassGenerator : ISourceGenerator
	{
		
		public void Execute(GeneratorExecutionContext context)
		{
			System.Console.WriteLine(System.DateTime.Now.ToString(CultureInfo.InvariantCulture));
			
			var callingEntrypoint = context.Compilation.GetEntryPoint(context.CancellationToken);

			var trees = context.Compilation.SyntaxTrees;
			var walker = new ClassCollector();
			foreach (var tree in trees)
			{
				var root = tree.GetCompilationUnitRoot();
				walker.Visit(root);
			}
			if (walker.Classes.Count <= 0) return;
			
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
			writer.WriteLine("public string[] listOfClasses = new[]");
			writer.BeginBlock();
			for (var index = 0; index < walker.Classes.Count; index++)
			{
				var @class = walker.Classes[index];
				var end = index == walker.Classes.Count - 1 ? "" : ",";
				writer.WriteLine($"{@class}{end}");
			}
			writer.EndBlock(";");
			writer.WriteLine("public void OnEnable()");
			writer.BeginBlock();
			writer.WriteLine("Debug.Log(\"OnEnable\");");
			writer.EndBlock();
			writer.EndBlock();
			writer.EndBlock();
			context.AddSource("TestComponent.generated.cs", SourceText.From(writer.ToString(), Encoding.UTF8));
		}

		public void Initialize(GeneratorInitializationContext context) { }
	}

	public class ClassCollector : CSharpSyntaxWalker
	{
		public readonly List<string> Classes = new List<string>();
		
		public override void VisitClassDeclaration(ClassDeclarationSyntax node)
		{
			base.VisitClassDeclaration(node);
			var attributes = node.AttributeLists.FirstOrDefault()?.Attributes;
			if(attributes == null) return;
			var foundMergeAttribute = default(AttributeSyntax);
			foreach (var att in attributes)
			{
				if (att.Name.ToString() == "MergeClasses")
				{
					foundMergeAttribute = att;
					break;
				}
			}
			if(foundMergeAttribute == null) return;

			var arguments = foundMergeAttribute.ArgumentList?.Arguments;
			if(arguments == null) return;
			foreach (var arg in arguments)
			{
				switch (arg.Expression)
				{
					case LiteralExpressionSyntax literal:
						Classes.Add(literal.Token.Text);
						break;
				}
				
			}
		}
	}
}