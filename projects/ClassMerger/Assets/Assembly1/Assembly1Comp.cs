using System.Collections.Generic;
using Needle;
using UnityEngine;

namespace Assembly1
{
	// Code doesnt compile if we have this line:
	// [MergeClass(typeof(PartialInDifferentAssembly))]
	[MergeClass(typeof(Assembly1Partial))]
	public partial class Assembly1Comp : MonoBehaviour
	{
		private void Start()
		{
			var list = this.PartialList;
		}
	}

	internal class Assembly1Partial
	{
		public List<string> PartialList;
	}
}