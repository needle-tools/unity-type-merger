using Unity.VisualScripting;

namespace MyNamespace
{
	public interface IMyInterface
	{
		void MyInterfaceMethod();
	}

	public class MyOtherType : IMyInterface
	{
		public string HelloWorld = "test123";
		
		public void MyInterfaceMethod()
		{
			
		}

		public void SomeMethod()
		{
			
		}
	}
}