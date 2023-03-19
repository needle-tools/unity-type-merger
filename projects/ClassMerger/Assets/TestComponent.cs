using System;
using Needle;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyNamespace
{
	[MergeClass("MyNamespace.MyOtherType", "MyNamespace.SomeOtherBehaviour")]
	[MergeClass("MyNamespace.SomeThirdType")]
	public partial class TestComponent
	{
		public string SomeString = "Hello World";

		private void Start()
		{
			
		} 
	}
}
