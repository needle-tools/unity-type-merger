using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnityAnalyzers
{
	internal class CurrentClassIdentifierReceiver : ISyntaxReceiver
	{
		public readonly MergeInfoCollector Collector = new();
		
		public bool HasClasses => Collector.Infos.Count > 0;

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			Collector.Visit(syntaxNode);
		}
	}
}