using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

// https://stackoverflow.com/a/49523780

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
				using var stream = Utils.LoadAssemblyForModule(assemblyPath);
				using var module = ModuleDefinition.ReadModule(stream, readerParameters);

				var myClass1 = module.Types.FirstOrDefault(t => t.Name == "MyClass1");
				var myComponent = module.Types.FirstOrDefault(t => t.Name == "MyComponent");

				CopyFromTo(myClass1, myComponent, module);

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

		private static void CopyFromTo(TypeDefinition source, TypeDefinition target, ModuleDefinition moduleDefinition)
		{
			var size = target.ClassSize;
			if (BuildPipeline.isBuildingPlayer)
				target.Fields.Add(new FieldDefinition("addedField", Mono.Cecil.FieldAttributes.Private,
					moduleDefinition.TypeSystem.String));
			
			foreach (var member in source.Fields)
			{
				target.Fields.AddSafe(member.Copy(moduleDefinition));
			}
			foreach (var member in source.Methods)
			{
				target.Methods.Add(member.Copy());
			}
			Debug.Log("Size before: " + size + ", after: " + target.ClassSize + ", Source? " + source.ClassSize);
		}
	}
}