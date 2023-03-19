using DifferentAssembly;
using Needle;
using Partials;
using UnityEngine;

namespace MyNamespace
{
	[MergeClass(typeof(MyOtherType), typeof(SomeOtherBehaviour))]
	[MergeClass(typeof(SomeThirdType))]
	// [MergeClass("DifferentAssembly.PartialInDifferentAssembly")]
	[MergeClass(typeof(PartialInDifferentAssembly))]
	public partial class TestComponent : MonoBehaviour
	{
		public string SomeString = "Hello World";
	}
}