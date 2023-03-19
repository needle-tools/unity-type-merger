using UnityEngine;
using UnityEngine.Serialization;

namespace MyNamespace
{
	public class SomeOtherBehaviour : MonoBehaviour
	{
		public bool Active = true;

		public void Update()
		{
			if (Active)
				Debug.Log("IT WORKS");
		}
	}
}