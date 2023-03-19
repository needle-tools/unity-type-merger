using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using UnityAnalyzers;

namespace Needle.ClassMerging
{
	[Generator]
	public class AttributeGenerator : ISourceGenerator
	{
		public const string AttributeName = "MergeClass";
		private const string AttributeClassName = AttributeName + "Attribute";
		
		public void Initialize(GeneratorInitializationContext context)
		{
			
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var writer = new CodeWriter();
			writer.WriteLine("using System;");
			writer.WriteLine("namespace Needle");
			writer.BeginBlock();
			writer.WriteLine("[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]");
			writer.WriteLine($"internal class {AttributeClassName} : System.Attribute");
			writer.BeginBlock();
			writer.WriteLine($"public {AttributeClassName}(params string[] types) {{ }}");
			writer.EndBlock();
			writer.EndBlock();
			var source = SourceText.From(writer.ToString(), Encoding.UTF8);
			context.AddSource("MergeClassAttribute.generated.cs", source);
		}
	}
}