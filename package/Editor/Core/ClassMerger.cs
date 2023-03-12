using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace Needle.ClassMerger.Core
{
	public abstract class ClassMerger
	{

		[MenuItem("TestWeave/Run")]
		[InitializeOnLoadMethod]
		private static void Init()
		{
			CompilationPipeline.assemblyCompilationFinished += ComplicationComplete;
			// AppDomain.CurrentDomain.AssemblyResolve += OnResolve;
			// AppDomain.CurrentDomain.AssemblyLoad += OnLoad;
			// CompilationPipeline.compilationStarted += OnCompilationStarted;

		} 

		// private static void OnLoad(object sender, AssemblyLoadEventArgs args)
		// {
		// 	if (sender is AppDomain domain)
		// 	{
		// 		domain.AssemblyLoad -= OnUnityDomainLoad;
		// 		domain.AssemblyLoad += OnUnityDomainLoad;
		// 	} 
		// }
		//
		// private static void OnUnityDomainLoad(object sender, AssemblyLoadEventArgs args)
		// {
		// 	
		// }
		//
		// private static void OnCompilationStarted(object obj)
		// {
		// 	
		// }
		//
		// private static Assembly OnResolve(object sender, ResolveEventArgs args)
		// {
		// 	return null;
		// }

		private static void ComplicationComplete(string assemblyPath, CompilerMessage[] arg2)
		{
			if (!assemblyPath.Contains("NewAssembly")) return;
			// while (EditorApplication.isCompiling || EditorApplication.isUpdating) await Task.Delay(100);
			// var assemblyPath = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp.dll";
			if(!File.Exists(assemblyPath)) return;
			try
			{
				// Lock assemblies while they may be altered
				EditorApplication.LockReloadAssemblies();
				
				var readerParameters = new ReaderParameters();
				readerParameters.ReadSymbols = true;
				readerParameters.SymbolReaderProvider = new PdbReaderProvider();
				// readerParameters.AssemblyResolver = assemblyResolver; 
				var stream = Utils.LoadAssemblyForModule(assemblyPath);
				var module = ModuleDefinition.ReadModule(stream, readerParameters);
				
				var myClass1 = module.Types.FirstOrDefault(t => t.Name == "MyClass1");
				var myComponent = module.Types.FirstOrDefault(t => t.Name == "MyComponent");

				CopyFromTo(myClass1, myComponent);
				
				var writerParameters = new WriterParameters();
				writerParameters.WriteSymbols = true; 
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
			}
		}

		private static void CopyFromTo(TypeDefinition source, TypeDefinition target)
		{
			// move all members
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
