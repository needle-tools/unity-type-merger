using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Needle.ClassMerging.Core
{
	public class ClassMergeInfo
	{
		public readonly string TargetClassNamespace;
		public readonly string TargetClassName;
		public readonly string SourceClassFullName;
		
		public ClassMergeInfo(string targetClassNamespace, string targetClassName, string sourceClassFullName)
		{
			this.TargetClassNamespace = targetClassNamespace;
			SourceClassFullName = sourceClassFullName;
			TargetClassName = targetClassName;
		}
		
		public readonly List<UsingDirectiveSyntax> imports = new();
		public readonly List<ClassDeclarationSyntax> classes = new();

		public void AddImport(UsingDirectiveSyntax i)
		{
			imports.Add(i);	
		}

		public void AddClass(ClassDeclarationSyntax c)
		{
			classes.Add(c);
		}
	}
}