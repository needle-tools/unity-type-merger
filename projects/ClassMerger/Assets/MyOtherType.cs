using Unity.VisualScripting;
using UnityEngine;

namespace MyNamespace
{
	public class MyOtherType
	{
		public string HelloWorld = "test";
	}


	public class MyOtherType2 : MonoBehaviour
	{
		public bool Test123 = true;

		public void Update()
		{
			Debug.Log("IT WORKS123");
		}
	}
}