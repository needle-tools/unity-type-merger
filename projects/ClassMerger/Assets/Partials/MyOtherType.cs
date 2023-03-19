using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyNamespace
{
	public interface IMyInterface
	{
		void MyInterfaceMethod();
	}

	public class MyOtherType : IMyInterface
	{
		[Header(nameof(MyOtherType))]
		public List<int> OtherTypeList;
		
		public void MyInterfaceMethod()
		{
			
		}

		public void SomeMethod()
		{
			
		}
	}
}