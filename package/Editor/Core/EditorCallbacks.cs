using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;

namespace Needle.ClassMerger.Core
{
	public class EditorCallbacks :IFilterBuildAssemblies
	{
		
		[MenuItem("TestWeave/Run")]
		[InitializeOnLoadMethod]
		private static void Init()
		{
			CompilationPipeline.assemblyCompilationFinished += ComplicationComplete;
			AssemblyReloadEvents.beforeAssemblyReload += OnBeforeReload;
			// AppDomain.CurrentDomain.AssemblyResolve += OnResolve;
			// AppDomain.CurrentDomain.AssemblyLoad += OnLoad;
			// CompilationPipeline.compilationStarted += OnCompilationStarted;

		}

		private static void OnBeforeReload()
		{
			if (BuildPipeline.isBuildingPlayer) return;
			var dlls = Directory.GetFiles("Library/ScriptAssemblies", "*.dll", SearchOption.AllDirectories);
			foreach (var dll in dlls)
			{
				if (dll.EndsWith(".dll"))
					ClassMerger.Process(dll);
			}
		}

		private static void ComplicationComplete(string assemblyPath, CompilerMessage[] arg2)
		{
			// if (BuildPipeline.isBuildingPlayer) return;
			// ClassMerger.Process(assemblyPath);
		}

		// public int callbackOrder { get; }
		//
		// public async void OnPostprocessBuild(BuildReport report)
		// {
		// 	await Task.Delay(10);
		// 	foreach (var file in report.files)
		// 	{
		// 		if(file.path.EndsWith(".dll"))
		// 			ClassMerger.Process(file.path);
		// 	}
		// }
		
		private static bool _HasProcessed;

		[PostProcessScene]
		private static void OnPostProcessScene()
		{
			if (_HasProcessed || !BuildPipeline.isBuildingPlayer)
				return;
			_HasProcessed = true;
			var dlls = Directory.GetFiles("Library/PlayerScriptAssemblies", "*.dll", SearchOption.AllDirectories);
			foreach (var dll in dlls)
			{
				if (dll.EndsWith(".dll"))
					ClassMerger.Process(dll);
			}
		}
		
		[PostProcessBuild]
		private static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
		{
			_HasProcessed = false;
		}

		public int callbackOrder { get; }
		public string[] OnFilterAssemblies(BuildOptions buildOptions, string[] assemblies)
		{
			// foreach (var assembly in assemblies)
			// {
			// 	if (assembly.EndsWith(".dll"))
			// 		ClassMerger.Process(assembly);
			// }

			return assemblies;
		}
	}
}