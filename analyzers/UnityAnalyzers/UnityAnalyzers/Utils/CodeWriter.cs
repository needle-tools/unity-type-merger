using System.Text;

namespace UnityAnalyzers
{
	public class CodeWriter
	{
		private int indent = 0;
		private readonly StringBuilder builder = new StringBuilder();
		
		public void BeginBlock()
		{
			WriteLine("{");
			indent++;
		}

		public void EndBlock(string postfix = null)
		{
			indent--;
			WriteLine("}" + postfix);
		}
		
		public void WriteLine(string line)
		{
			for (int i = 0; i < indent; i++) 
				builder.Append("\t");
			builder.AppendLine(line);
		}

		public override string ToString()
		{
			return builder.ToString();
		}
	}
}