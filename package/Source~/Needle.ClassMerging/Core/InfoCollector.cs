using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityAnalyzers;

namespace Needle.ClassMerging.Core
{
	public class InfoCollector : CSharpSyntaxWalker
	{
		private readonly GeneratorInitializationContext context;
		public readonly List<ClassMergeInfo> Infos = new();
		private readonly Stack<string> namespaces = new();

		public readonly CodeWriter debugWriter = new();

		public InfoCollector(GeneratorInitializationContext context)
		{
			this.context = context;
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

			foreach (var attributeListEntry in node.AttributeLists)
			{
				var attributes = attributeListEntry.Attributes;
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
								// Handle typeof
								case TypeOfExpressionSyntax typeOf:
									// TODO: make it work with typeof - we might need to resolve the full type namespace later
									var type = typeOf.Type;
									debugWriter.WriteLine("??? TypeOf: " + type);
									break;
							}
						}
					}
				}
			}

		}

		// public override void VisitAttributeArgument(AttributeArgumentSyntax node)
		// {
		// 	base.VisitAttributeArgument(node);
		// }
	}
}


