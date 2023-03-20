using System;
using System.IO;
using Microsoft.CodeAnalysis;

namespace Needle.ClassMerging.Utils
{
	public static class GeneratorExtensions
	{
		
		
		public static string? TryGetUnityProjectDirectory(this GeneratorExecutionContext context)
		{
			foreach (var file in context.AdditionalFiles)
			{
				const string libraryKeyword = "Library";
				var path = file.Path;
				var index = path.IndexOf(libraryKeyword, StringComparison.Ordinal);
				if (index > 0)
					return path.Substring(0, index);
			}

			return null;
		}
		
		private static string GetLogOutputPath(this GeneratorExecutionContext context)
		{
			var projectDirectory = context.TryGetUnityProjectDirectory();
			if (!string.IsNullOrEmpty(projectDirectory))
				return Path.Combine(projectDirectory, "Temp", "SourceGenerator", "Needle");
			return Path.Combine(Path.GetTempPath(), "Needle", "SourceGenerator");
		}

		private static string? GetGeneratedDebugPath(this GeneratorExecutionContext context)
		{
			string logOutputPath = context.GetLogOutputPath();
			if (string.IsNullOrEmpty(logOutputPath))
				return null;
			var path = Path.Combine(logOutputPath);
			Directory.CreateDirectory(path);
			return path;
		}
		
		public static void LogInfo(this GeneratorExecutionContext context, string message)
		{
			var generatedDebugPath = context.GetGeneratedDebugPath();
			if (string.IsNullOrEmpty(generatedDebugPath))
				return;
			try
			{
				using var streamWriter = File.AppendText(Path.Combine(generatedDebugPath, "SourceGen.md"));
				streamWriter.WriteLine(message);
			}
			catch (IOException)
			{
			}
		}

		public static void LogError(this GeneratorExecutionContext context, string message)
		{
			LogInfo(context, "ERROR: " + message);
		}
	}
}