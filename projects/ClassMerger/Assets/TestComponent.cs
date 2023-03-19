using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyNamespace
{
	// [MergeClasses("MyNamespace.TestComponent")]
	[MergeClasses("MyNamespace.MyOtherType", "MyNamespace.SomeOtherBehaviour")]
	public partial class TestComponent : MonoBehaviour
	{
		public string SomeString = "Hello World";
	}
}
