# Unity Type Merger

Merge multiple classes into one using SourceGenerators

> ⚠ This is very experimental

## How it works:

1) Mark your class or component as `partial`
2) Add one or multiple `MergeClass` attributes to your class
3) Insert a list of type names that you want to be merged into that class or component 


Example Class:
```csharp
namespace MyNamespace
{
	[MergeClass("MyNamespace.MyOtherType", "MyNamespace.SomeOtherBehaviour")]
	[MergeClass("Partials.SomeThirdType")]
	public partial class TestComponent
	{
		public string SomeString = "Hello World";
	}
}

```

Resulting Sourcegen:
```csharp
namespace MyNamespace
{
	public partial class TestComponent : MonoBehaviour, IMyInterface // ScriptableObject
	{
		// Begin MyOtherType
		public string HelloWorld = "test123";
		public void MyInterfaceMethod()
		{
			
		}
		// End MyOtherType

		// Begin SomeOtherBehaviour
		public bool Active = true;
		public void Update()
		{
			if (Active)
				Debug.Log("IT WORKS");
		}
		// End SomeOtherBehaviour

		// Begin SomeThirdType
		public void MySpecialMethod()
		{
			Debug.Log("Hello from some third type");
		}
		// End SomeThirdType

	}
}
```