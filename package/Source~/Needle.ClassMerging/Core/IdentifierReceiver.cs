using Microsoft.CodeAnalysis;

namespace Needle.ClassMerging.Core
{
	internal class IdentifierReceiver : ISyntaxReceiver
	{
		public readonly InfoCollector Collector = new();
		
		public bool HasClasses => Collector.Infos.Count > 0;

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			Collector.Visit(syntaxNode);
		}
	}
}