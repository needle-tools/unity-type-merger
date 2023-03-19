using DifferentAssembly;
using Needle;
using UnityEngine;

namespace MyNamespace
{
	// [MergeClass("MyNamespace.MyOtherType", "MyNamespace.SomeOtherBehaviour")]
	[MergeClass("Partials.SomeThirdType")]
	[MergeClass(typeof(MyOtherType))]
	// [MergeClass("DifferentAssembly.PartialInDifferentAssembly")]
	// [MergeClass(typeof(PartialInDifferentAssembly))]
	public partial class TestComponent : MonoBehaviour
	{
		public string SomeString = "Hello World";     
	} 
}
