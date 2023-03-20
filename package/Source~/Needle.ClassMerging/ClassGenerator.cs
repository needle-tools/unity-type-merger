using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Needle.ClassMerging.Core;
using Needle.ClassMerging.Utils;

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
			
#if DEBUG
			if (!Debugger.IsAttached)
			{
				Debugger.Launch();
			}
#endif

			var watch = Stopwatch.StartNew();
			
			context.LogInfo("\n// " + DateTime.Now.ToString("HH:mm:ss"));
			context.LogInfo("--> ClassGenerator.Execute() for " +
			                string.Join(", ", receiver.Collector.Infos.Select(i => i.TargetClassName)));
			context.LogInfo("Assembly " + context.Compilation.AssemblyName);

			var collector = receiver.Collector;
			var classInfos = collector.Infos;

			var debugWriter = new CodeWriter();
			var wr = new ClassWriter(debugWriter);
			wr.CollectInfos(context, classInfos);


			var writer = new CodeWriter();
			foreach (var info in wr.WriteInfos(context, classInfos, writer))
			{
				var raw = writer.ToString();
				
// 				raw = @"
// using System.Collections.Generic;
//
// namespace Assembly1
// {
// 	public partial class Assembly1Comp
// 	{
// 		public List<string> PartialList; 
// 	}
// }
//
//
// ";

				
				var filename = $"{info}.generated.cs";
// #if DEBUG
				context.LogInfo("DEBUG INFO:");
				context.LogInfo(collector.debugWriter.ToString());
				context.LogInfo(debugWriter.ToString());
				context.LogInfo("CODEGEN START: " + filename);
				context.LogInfo("```csharp");
				context.LogInfo(raw);
				context.LogInfo("```");
				context.LogInfo("CODEGEN END: " + filename);
// #endif		
				var code = SourceText.From(raw, Encoding.UTF8);

				
				context.AddSource(filename, code);
				writer.Clear();
				context.LogInfo("Written " + filename + " in " + watch.ElapsedMilliseconds + "ms");
			}
		}
	}
}