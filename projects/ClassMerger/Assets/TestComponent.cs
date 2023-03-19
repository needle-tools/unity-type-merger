using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MyNamespace
{
	[MergeClasses("MyNamespace.MyOtherType", "MyNamespace.MyOtherType2")]
	public partial class TestComponent : MonoBehaviour
	{
		public string Test1234 = "Hello Zach";    
	}
}
