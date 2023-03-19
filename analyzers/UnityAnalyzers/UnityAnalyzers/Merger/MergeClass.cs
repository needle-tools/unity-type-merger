using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace UnityAnalyzers
{
	// https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#augment-user-code
	// https://www.infoq.com/articles/CSharp-Source-Generator/


	[Generator]
	public class MergedClassGenerator : ISourceGenerator
	{
		// https://github.com/needle-mirror/com.unity.entities/blob/2b7ad3ab445aff771ddffa3dd9d330f21fb1dd70/Unity.Entities/SourceGenerators/Source~/SystemGenerator/SystemGenerator.cs#L20


		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new CurrentClassIdentifierReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var receiver = (CurrentClassIdentifierReceiver)context.SyntaxReceiver!;
			if (!receiver.HasClasses) return;

			var classInfos = receiver.Collector.Infos;


			var debugWriter = new CodeWriter();
			var wr = new MergeClassWriter(debugWriter);
			wr.CollectInfos(context, classInfos);


			var writer = new CodeWriter();
			foreach (var info in wr.WriteInfos(classInfos, writer))
			{
				debugWriter.WriteLine(writer.ToString());
				var code = SourceText.From(writer.ToString(), Encoding.UTF8);
				context.AddSource($"{info}.generated.cs", code);
				writer.Clear();
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