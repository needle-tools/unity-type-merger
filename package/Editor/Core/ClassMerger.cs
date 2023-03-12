using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using UnityEditor;
using UnityEngine;

namespace Needle.ClassMerger.Core
{
	public static class ClassMerger
	{
		internal static void Process(string assemblyPath)
		{
			if (!assemblyPath.Contains("NewAssembly")) return;
			// while (EditorApplication.isCompiling || EditorApplication.isUpdating) await Task.Delay(100);
			// var assemblyPath = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp.dll";
			if (!File.Exists(assemblyPath)) return;
			try
			{
				Debug.Log("PATCH " + assemblyPath);
				// Lock assemblies while they may be altered
				EditorApplication.LockReloadAssemblies();

				var readerParameters = new ReaderParameters();
				var symbolsPath = Path.ChangeExtension(assemblyPath, ".pdb");
				var hasSymbols = File.Exists(symbolsPath);
				readerParameters.ReadSymbols = hasSymbols;
				if (hasSymbols)
					readerParameters.SymbolReaderProvider = new PdbReaderProvider();
				// readerParameters.AssemblyResolver = assemblyResolver; 
				var stream = Utils.LoadAssemblyForModule(assemblyPath);
				var module = ModuleDefinition.ReadModule(stream, readerParameters);

				var myClass1 = module.Types.FirstOrDefault(t => t.Name == "MyClass1");
				var myComponent = module.Types.FirstOrDefault(t => t.Name == "MyComponent");

				CopyFromTo(myClass1, myComponent);

				var writerParameters = new WriterParameters();
				writerParameters.WriteSymbols = hasSymbols;
				if (hasSymbols)
					writerParameters.SymbolWriterProvider = new PdbWriterProvider();
				module.Write(stream, writerParameters);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			finally
			{
				EditorApplication.UnlockReloadAssemblies();
				AssetDatabase.Refresh();
				EditorUtility.RequestScriptReload();
			}
		}

		private static void CopyFromTo(TypeDefinition source, TypeDefinition target)
		{
			foreach (var member in source.Fields)
			{
				target.Fields.AddSafe(member.Copy());
			}
			foreach (var member in source.Methods)
			{
				target.Methods.Add(member.Copy());
			}
		}
	}
}