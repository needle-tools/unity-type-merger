using Needle;

namespace MyNamespace
{
	[MergeClass("MyNamespace.MyOtherType", "MyNamespace.SomeOtherBehaviour")]
	[MergeClass("Partials.SomeThirdType")]
	[MergeClass("DifferentAssembly.PartialInDifferentAssembly")]
	[MergeClass(typeof(ReferencingTestComponent))]
	public partial class TestComponent
	{
		public string SomeString = "Hello World";
	}
}
