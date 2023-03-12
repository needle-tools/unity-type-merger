using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MyNamespace
{
	[MergeClasses("UnityEngine.MonoBehaviour", "MyOtherType")]
	public partial class TestComponent : MonoBehaviour
	{
		public string Test123 = "Hello Zach";
	}

	public class MergeClassesAttribute : Attribute
	{
		public MergeClassesAttribute(params string[] types)
		{
		}
	}
}
