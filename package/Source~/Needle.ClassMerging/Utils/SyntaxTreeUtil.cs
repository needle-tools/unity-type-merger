using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Needle.ClassMerging.Utils;

namespace Needle.ClassMerging
{
	public class SyntaxTreeUtil
	{
		private readonly GeneratorExecutionContext context;
		private readonly TypesCache? cache;

		public SyntaxTreeUtil(GeneratorExecutionContext context)
		{
			this.context = context;
			var projectDir = context.TryGetUnityProjectDirectory();
			if (projectDir != null)
				cache = new TypesCache(projectDir, context);
		}

		public SyntaxTree? TryFindTree(string typeName)
		{
			var source = cache?.TryFindSource(typeName);
			if (!string.IsNullOrEmpty(source))
				return GetFromSource(source!);

			return null;
		}

		private static SyntaxTree GetFromSource(string source)
		{
			return CSharpSyntaxTree.ParseText(source);
		}
	}

	internal class TypesCache
	{
		private readonly string rootDirectory;
		private readonly GeneratorExecutionContext context;

		public TypesCache(string rootDirectory, GeneratorExecutionContext context)
		{
			this.rootDirectory = rootDirectory;
			this.context = context;
		}
		
		internal string? TryFindSource(string name)
		{
			InitCache();
			// We currently only find types that have the same name as the file
			if (!name.EndsWith(".cs")) name += ".cs";
			foreach (var file in scriptFilePaths!)
			{
				if (file.EndsWith(name))
				{
					context.LogInfo("Found source for " + name + " at " + file);
					return File.ReadAllText(file);
				}
			}

			return null;
		}


		private string[]? scriptFilePaths = null;

		private void InitCache()
		{
			if (scriptFilePaths != null) return;
			var watch = System.Diagnostics.Stopwatch.StartNew();
			var assetsDirectory = rootDirectory + "/Assets";
			var assetsScripts = Directory.GetFiles(assetsDirectory, "*.cs", SearchOption.AllDirectories);
			var packageDirectory = rootDirectory + "/Library/PackageCache";
			var packageScripts = Directory.GetFiles(packageDirectory, "*.cs", SearchOption.AllDirectories);
			scriptFilePaths = new string[assetsScripts.Length + packageScripts.Length];
			assetsScripts.CopyTo(scriptFilePaths, 0);
			packageScripts.CopyTo(scriptFilePaths, assetsScripts.Length);
			watch.Stop();
			context.LogInfo("Found " + scriptFilePaths.Length + " scripts in " + watch.ElapsedMilliseconds + "ms");
		}
	}
}