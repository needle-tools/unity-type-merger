using UnityEngine;

namespace MyNamespace
{
	public class ReferencingTestComponent : MonoBehaviour
	{
		public TestComponent Comp;

		public void Start()
		{
			Comp?.MySpecialMethod();
		}
	}
}