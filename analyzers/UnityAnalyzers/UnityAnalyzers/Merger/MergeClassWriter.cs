﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnityAnalyzers
{
	public class MergeClassWriter
	{
		private readonly CodeWriter debug;

		public MergeClassWriter(CodeWriter debug)
		{
			this.debug = debug;
		}

		public void CollectInfos(GeneratorExecutionContext context, IEnumerable<ClassMergeInfo> infos)
		{
			foreach (var info in infos)
			{
				foreach (var tree in context.Compilation.SyntaxTrees)
				{
					// If we found the class we dont need to continue
					var root = tree.GetCompilationUnitRoot();
					var visitor = new ClassFinder(info, debug);
					visitor.Visit(root);
					if (visitor.Result) break;
				}
			}
		}

		public IEnumerable<string> WriteInfos(IEnumerable<ClassMergeInfo> infos, CodeWriter writer)
		{
			var infosByTarget = infos.GroupBy(i => i.TargetClassName).ToDictionary(g => g.Key, g => g.ToList());

			foreach (var info in infosByTarget)
			{
				var data = info.Value;
				
				var targetClassInfo = data.FirstOrDefault();
				if (targetClassInfo == null) continue;
				
				var imported = new HashSet<string>();
				foreach (var entry in data)
				{
					foreach (var i in entry.imports)
					{
						var importString = i.ToString();
						if (imported.Contains(importString)) continue;
						imported.Add(importString);
						writer.WriteLine(importString);
					}
				}
				
				writer.WriteLine();

				writer.WriteLine("namespace " + targetClassInfo.TargetClassNamespace);
				writer.BeginBlock();
				
				writer.WriteLine($"public partial class {targetClassInfo.TargetClassName}");
				writer.BeginBlock();
				foreach (var entry in data)
				{
					foreach (var c in entry.classes)
					{
						foreach (var mem in c.Members)
						{
							writer.WriteLine(mem.ToString());
						}
					}
				}
				writer.EndBlock();
				
				writer.EndBlock();
				
				yield return info.Key;
			}
		}
	}

	public class ClassFinder : CSharpSyntaxWalker
	{
		public bool Result { get; private set; }
		
		private readonly ClassMergeInfo info;
		private readonly CodeWriter debug;

		public ClassFinder(ClassMergeInfo info, CodeWriter debug)
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
			
			debug.WriteLine(fullName + " ?= " + info.SourceClassFullName);
			// is it the full name we search?
			if (fullName == info.SourceClassFullName)
			{
				debug.WriteLine("> FOUND FOR " + info.TargetClassName + "!");
				info.AddClass(node);
				Result = true;
			}
		}
	}
}