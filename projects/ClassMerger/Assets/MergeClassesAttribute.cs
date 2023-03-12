using System;

namespace MyNamespace
{
	public class MergeClassesAttribute : Attribute
	{
		public MergeClassesAttribute(params string[] types)
		{
		}
	}
}