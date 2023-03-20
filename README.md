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

Final component:  
![image](https://user-images.githubusercontent.com/5083203/226190153-589ec52c-6ba7-4693-9c1f-5204c03b87a7.png)


## Limitations
- Multiple base classes are not supported (e.g. for multiple partials deriving from different classes only the first one will be used)
- If the main class already has a base type and a partial also has a base type it will cause compiler errors (Sourcegen currently doesnt check that)
- If partials have member name collisions (e.g. two classes declaring a method name `MySpecialMethod` it will cause compiler errors). 
- Types to merge must be in the same assembly at the moment


## Debugging
- See https://github.com/needle-tools/unity-analyzers-starter#debugging
