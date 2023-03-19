using UnityEngine;

namespace Partials
{
	public class SomeThirdType// : ScriptableObject
	{
		[Header("TEST HEADER")]
		public string SomeThirdTypeString = "Hello World";
		
		[ContextMenu(nameof(MySpecialMethod))]
		public void MySpecialMethod()
		{
			Debug.Log("Hello from some third type");
		}

		// It doesnt work if multiple types have the same method name
		// public void SomeMethod()
		// {
		// 	
		// }
	}
}