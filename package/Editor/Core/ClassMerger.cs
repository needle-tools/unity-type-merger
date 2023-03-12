using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fody;
using Mono.Cecil;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;
using FieldAttributes = Mono.Cecil.FieldAttributes;

namespace Needle.ClassMerger.Core
{
	public abstract class ClassMerger
	{

		[MenuItem("TestWeave/Run")]
		[InitializeOnLoadMethod]
		private static async void Init()
		{
			CompilationPipeline.assemblyCompilationFinished += ComplicationComplete;
			
		}

		private static void ComplicationComplete(string assemblyPath, CompilerMessage[] arg2)
		{
			if (!assemblyPath.Contains("Assembly-CSharp")) return;
			// while (EditorApplication.isCompiling || EditorApplication.isUpdating) await Task.Delay(100);
			// var assemblyPath = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp.dll";
			if(!File.Exists(assemblyPath)) return;
			
			
			try
			{
				// Lock assemblies while they may be altered
				EditorApplication.LockReloadAssemblies();
				
				var readerParameters = new ReaderParameters();
				// readerParameters.AssemblyResolver = assemblyResolver; 
				var stream = Utils.LoadAssemblyForModule(assemblyPath);
				var module = ModuleDefinition.ReadModule(stream, readerParameters);
				
				
				var weaver = new Weaver();
				weaver.ModuleDefinition = module;
				weaver.Execute(); 

				var writerParameters = new WriterParameters(); 
			
				module.Write(stream, writerParameters); 
				
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			finally
			{
				EditorApplication.UnlockReloadAssemblies();
				UnityEditor.AssetDatabase.Refresh();
			}
		}

		private static void Process(Assembly assembly)
		{
			var readerParameters = new ReaderParameters();
			// assembly.GetModule()
		}
	}

	internal class Weaver : BaseModuleWeaver
	{
		public override void Execute()
		{
			foreach (var type in ModuleDefinition.Types)
			{
				if (type.Name == "MyComponent")
				{
					if (type.Fields.Any(f => f.Name == "MyStringField")) continue;
					type.Fields.Add(new FieldDefinition("MyStringField", FieldAttributes.Public, ModuleDefinition.TypeSystem.String));
				}
			}

		}

		public override IEnumerable<string> GetAssembliesForScanning()
		{
			return new[] {"Assembly-CSharp.dll"};
		}
	}
}
