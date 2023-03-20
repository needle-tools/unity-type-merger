using DifferentAssembly;
using Needle;
using UnityEngine;

namespace Assembly1
{
	// Code doesnt compile if we have this line:
	[MergeClass(typeof(PartialInDifferentAssembly))]
	[MergeClass(typeof(Assembly1Partial))]
	// [MergeClass("Assembly1.Assembly1Partial")]
	public partial class Assembly1Comp : MonoBehaviour
	{ 
		private void OnValidate()
		{
			var list = this.PartialList;
			foreach (var entry in list)
			{
				Debug.Log(entry); 
			}
		}
	}
}

