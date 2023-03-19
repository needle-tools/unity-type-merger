using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityAnalyzers;

namespace Needle.ClassMerging.Core
{
	public class ClassInfoWalker : CSharpSyntaxWalker
	{
		public bool Result { get; private set; }
		
		private readonly ClassMergeInfo info;
		private readonly CodeWriter debug;

		public ClassInfoWalker(ClassMergeInfo info, CodeWriter debug)
		{
			this.info = info;
			this.debug = debug;
		}

		private readonly Stack<string> namespaces = new();

		public override void VisitUsingDirective(UsingDirectiveSyntax node)
		{
			base.VisitUsingDirective(node);
			info.AddImport(node);
		}

		public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
		{
			namespaces.Push(node.Name.ToString());
			base.VisitNamespaceDeclaration(node);
			namespaces.Pop();
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node)
		{
			base.VisitClassDeclaration(node);
			// create namespace
			var fullName = string.Join(".", namespaces);
			if (fullName.Length > 0) fullName += ".";
			fullName += node.Identifier.ToString();
			
			debug.WriteLine("SEARCH:\t" + info.SourceClassFullName + " is " + fullName + "?");
			// is it the full name we search?
			if (fullName == info.SourceClassFullName)
			{
				debug.WriteLine("> FOUND\tFOR " + info.TargetClassName + "!");
				info.AddClass(node);
				Result = true;
			}
		}
	}
}