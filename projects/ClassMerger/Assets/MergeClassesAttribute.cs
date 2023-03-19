using System;

namespace MyNamespace
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class MergeClassesAttribute : Attribute
	{
		public MergeClassesAttribute(params string[] types)
		{
		}
		
		public MergeClassesAttribute(params Type[] types)
		{
		}
	}
}