using System;
using DifferentAssembly;
using Needle;
using Partials;
using UnityEngine;

namespace MyNamespace
{
	[MergeClass(typeof(MyOtherType), typeof(SomeOtherBehaviour))]
	[MergeClass(typeof(SomeThirdType))]
	// [MergeClass(typeof(PartialInDifferentAssembly))]
	public partial class TestComponent : MonoBehaviour
	{
		public string SomeString = "Hello World";

		private void OnEnable()
		{
			
		}
	}
}