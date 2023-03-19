using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Needle.ClassMerging.Core
{
	public class InfoCollector : CSharpSyntaxWalker
	{
		public readonly List<ClassMergeInfo> Infos = new();

		private readonly Stack<string> namespaces = new();

		public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
		{
			namespaces.Push(node.Name.ToString());
			base.VisitNamespaceDeclaration(node);
			namespaces.Pop();
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node)
		{
			base.VisitClassDeclaration(node);
			var attributes = node.AttributeLists.FirstOrDefault()?.Attributes;
			if (attributes == null) return;
			foreach (var att in attributes)
			{
				if (att.Name.ToString() == AttributeGenerator.AttributeName)
				{
					var arguments = att.ArgumentList?.Arguments;
					if (arguments == null) continue;

					var targetClassName = node.Identifier.Text;
					var targetClassNamespace = string.Join(".", namespaces);

					foreach (var arg in arguments)
					{
						switch (arg.Expression)
						{
							// Handle params as string
							case LiteralExpressionSyntax literal:
								var name = literal.Token.Text.Trim('\"');
								if (!Infos.Any(i => i.SourceClassFullName == name))
									Infos.Add(new ClassMergeInfo(targetClassNamespace, targetClassName, name));
								break;
						}
					}
				}
			}

		}
	}
}