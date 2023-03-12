using System.IO;
using UnityEngine;

namespace Needle.ClassMerger.Core
{
	public static class Utils
	{
		public static Stream LoadAssemblyForModule(string assemblyPath, bool inMemory = false)
		{
			if (!inMemory)
			{
				return new FileStream(assemblyPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			}
        
			// TODO: this does not work yet (BadImageException from Cecil ImageReader.ReadImage:63
			var rawAssembly = File.ReadAllBytes(assemblyPath);
			// to make it expandable https://stackoverflow.com/a/52052656
			var ms = new MemoryStream(0);
			ms.Write(rawAssembly, 0, rawAssembly.Length);
			return ms;
			// return new MemoryStream(rawAssembly, true);
		}
	}
}