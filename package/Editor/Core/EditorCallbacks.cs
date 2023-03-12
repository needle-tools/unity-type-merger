using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Compilation;

namespace Needle.ClassMerger.Core
{
	public class EditorCallbacks : IPostprocessBuildWithReport
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

		private static void ComplicationComplete(string assemblyPath, CompilerMessage[] arg2)
		{
			if (BuildPipeline.isBuildingPlayer) return;
			ClassMerger.Process(assemblyPath);
		}

		public int callbackOrder { get; }
		
		public async void OnPostprocessBuild(BuildReport report)
		{
			await Task.Delay(10);
			foreach (var file in report.files)
			{
				if(file.path.EndsWith(".dll"))
					ClassMerger.Process(file.path);
			}
		}
	}
}