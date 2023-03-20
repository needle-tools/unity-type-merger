using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Needle.ClassMerging.Utils;

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
									var type = typeOf.Type;
									if (!Infos.Any(i => i.SourceTypeSyntax == type))
									{
										debugWriter.WriteLine("??? TypeOf: " + type);
										// find type using
										var info = new ClassMergeInfo(targetClassNamespace, targetClassName, null);
										info.SourceTypeSyntax = type;
										Infos.Add(info);
									}
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


