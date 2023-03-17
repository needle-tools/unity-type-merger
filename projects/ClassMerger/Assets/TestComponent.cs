using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MyNamespace
{
	[MergeClasses("Type.DoesntExist", "MyNamespace.MyOtherType")]
	public partial class TestComponent : MonoBehaviour
	{
		public string Test123 = "Hello Zach";    
	}
}
