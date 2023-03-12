using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class MyComponent : MonoBehaviour
	{     
		public string Hello = "Hello";

		private void Start()
		{
			Debug.Log(this);
		}
	}
}