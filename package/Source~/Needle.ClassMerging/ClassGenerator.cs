using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Needle.ClassMerging.Core;
using UnityAnalyzers;

namespace Needle.ClassMerging
{
	// https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#augment-user-code
	// https://www.infoq.com/articles/CSharp-Source-Generator/


	[Generator]
	public class ClassGenerator : ISourceGenerator
	{
		// https://github.com/needle-mirror/com.unity.entities/blob/2b7ad3ab445aff771ddffa3dd9d330f21fb1dd70/Unity.Entities/SourceGenerators/Source~/SystemGenerator/SystemGenerator.cs#L20
		
		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new IdentifierReceiver(context));
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var receiver = (IdentifierReceiver)context.SyntaxReceiver!;
			if (!receiver.HasClasses) return;

			var collector = receiver.Collector;
			var classInfos = collector.Infos;
			
			var debugWriter = new CodeWriter();
			var wr = new ClassWriter(debugWriter);
			wr.CollectInfos(context, classInfos);


			var writer = new CodeWriter();
			var foundAny = false;
			foreach (var info in wr.WriteInfos(context, classInfos, writer))
			{
				foundAny = true;
				writer.WriteLine("/* DEBUG:");
				writer.WriteLine(collector.debugWriter.ToString() + "\n");
				writer.WriteLine(debugWriter.ToString());
				writer.WriteLine("*/");
				
				var code = SourceText.From(writer.ToString(), Encoding.UTF8);
				context.AddSource($"{info}.generated.cs", code);
				writer.Clear();
			}
			if (!foundAny)
			{
				// provide some debug info
			}

			// writer.WriteLine($"// Generated at {DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
			// writer.WriteLine($"// Assembly: {context.Compilation.AssemblyName}");
			// writer.WriteLine($"// SourceModule: {context.Compilation.SourceModule.Name}");
			// writer.WriteLine($"// ScriptClass: {context.Compilation.ScriptClass?.Name}");
			// writer.WriteLine($"// Classes: {string.Join(", ", receiver.Collector.Infos)}");
			// writer.WriteLine( $"// ContainingNamespace: {context.Compilation.ScriptClass?.ContainingNamespace?.Name}");
			// // writer.WriteLine($"// CallingEntryPoint: {callingEntrypoint?.Name}");
			// writer.WriteLine($"// AdditionalFiles: {string.Join(", ", context.AdditionalFiles)}");
			// writer.WriteLine();
			//
			// writer.WriteLine("/*");
			// writer.WriteLine("//DEBUG");
			// writer.WriteLine(debugWriter.ToString());
			// writer.WriteLine("//END DEBUG");
			// writer.WriteLine("*/\n");
			//
			// writer.WriteLine("using System;");
			// writer.WriteLine("using UnityEngine;");
			// writer.WriteLine($"\nnamespace MyNamespace");// {callingEntrypoint!.ContainingNamespace.ContainingNamespace.Name}.{callingEntrypoint!.ContainingNamespace.Name}");
			// writer.BeginBlock();
			// writer.WriteLine("public partial class TestComponent : MonoBehaviour");
			// writer.BeginBlock();
			//
			// writer.EndBlock();
			// writer.EndBlock();
			// context.AddSource("TestComponent.generated.cs", SourceText.From(writer.ToString(), Encoding.UTF8));
		}
	}
}