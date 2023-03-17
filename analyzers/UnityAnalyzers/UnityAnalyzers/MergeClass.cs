using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace UnityAnalyzers
{
	// https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#augment-user-code
	// https://www.infoq.com/articles/CSharp-Source-Generator/

	[Generator]
	public class MergedClassGenerator : ISourceGenerator
	{
		private class CurrentClassIdentifierReceiver : ISyntaxReceiver
		{
			// public readonly List<ClassDeclarationSyntax> ReceivedClasses = new();

			public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
			{
				// if (syntaxNode is ClassDeclarationSyntax cds)
				// {
				// 	ReceivedClasses.Add(cds);
				// }
			}
		}
		
		// https://github.com/needle-mirror/com.unity.entities/blob/2b7ad3ab445aff771ddffa3dd9d330f21fb1dd70/Unity.Entities/SourceGenerators/Source~/SystemGenerator/SystemGenerator.cs#L20
		// public void Initialize(GeneratorInitializationContext context)
		// {
		// 	// Registering this here causes errors? Execute doesnt run anymore
		// 	context.RegisterForSyntaxNotifications(this.OnSyntaxNotifications);
		// 	
		// 	context.RegisterForPostInitialization((c) =>
		// 	{
		// 	});
		// }
		//
		// private ISyntaxContextReceiver? OnSyntaxNotifications()
		// {
		// 	return null;
		// }

		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new CurrentClassIdentifierReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{

			// var callingEntrypoint = context.Compilation.GetEntryPoint(context.CancellationToken);

			var trees = context.Compilation.SyntaxTrees;
			var typeInfoCollector = new TypesToMergeCollector();
			foreach (var tree in trees)
			{
				var root = tree.GetCompilationUnitRoot();
				typeInfoCollector.Visit(root);
			}
			if (typeInfoCollector.Classes.Count <= 0) return;
			
			var writer = new CodeWriter();
			
			writer.WriteLine("using System;");
			writer.WriteLine("using UnityEngine;");
			writer.WriteLine("#if UNITY_EDITOR");
			writer.WriteLine("using UnityEditor;");
			writer.WriteLine("#endif\n");
			
			writer.WriteLine($"// Generated at {DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
			writer.WriteLine($"// Assembly: {context.Compilation.AssemblyName}");
			writer.WriteLine($"// SourceModule: {context.Compilation.SourceModule.Name}");
			writer.WriteLine($"// ScriptClass: {context.Compilation.ScriptClass?.Name}");
			writer.WriteLine($"// Classes: ");
			writer.WriteLine($"// ContainingNamespace: {context.Compilation.ScriptClass?.ContainingNamespace?.Name}");
			// writer.WriteLine($"// CallingEntryPoint: {callingEntrypoint?.Name}");
			writer.WriteLine($"// AdditionalFiles: {string.Join(", ", context.AdditionalFiles)}");
			
			writer.WriteLine($"\nnamespace MyNamespace");// {callingEntrypoint!.ContainingNamespace.ContainingNamespace.Name}.{callingEntrypoint!.ContainingNamespace.Name}");
			writer.BeginBlock();
			writer.WriteLine("public partial class TestComponent : MonoBehaviour");
			writer.BeginBlock();
			writer.WriteLine("public string mySourceGen = \"Hi From Sourcegen\";");
			// typeInfoCollector.GenerateDebugInfo(writer);
			writer.EndBlock();
			writer.EndBlock();
			context.AddSource("TestComponent.generated.cs", SourceText.From(writer.ToString(), Encoding.UTF8));
		}

	}

	public class TypesToMergeCollector : CSharpSyntaxWalker
	{
		internal void GenerateDebugInfo(CodeWriter writer)
		{
			if(Classes?.Count > 0)
				writer.WriteLine("// Found to merge: " + string.Join(", ", Classes));
		}
		
		public readonly List<string> Classes = new();
		
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
	
	
	// writer.WriteLine("public string[] listOfClasses = new[]");
	// writer.BeginBlock();
	// for (var index = 0; index < walker.Classes.Count; index++)
	// {
	// 	var @class = walker.Classes[index];
	// 	var end = index == walker.Classes.Count - 1 ? "" : ",";
	// 	writer.WriteLine($"{@class}{end}");
	// }
	// writer.EndBlock(";");
}