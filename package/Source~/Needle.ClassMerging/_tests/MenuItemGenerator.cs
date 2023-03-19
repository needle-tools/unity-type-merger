// using System.Text;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.Text;
//
// namespace UnityAnalyzers
// {
// 	[Generator]
// 	public class MenuItemGenerator : ISourceGenerator
// 	{
// 		private static bool _didRun = false;
// 		
// 		public void Execute(GeneratorExecutionContext context)
// 		{
// 			if (_didRun) return;
// 			_didRun = true;
// 			var writer = new CodeWriter();
// 			writer.WriteLine("using System;");
// 			writer.WriteLine("using UnityEngine;");
// 			writer.WriteLine("#if UNITY_EDITOR");
// 			writer.WriteLine("using UnityEditor;");
// 			writer.WriteLine("#endif");
// 			writer.WriteLine($"namespace Needle.Test");
// 			writer.BeginBlock();
// 			writer.WriteLine("internal static class MyMenuItem");
// 			writer.BeginBlock();
// 			writer.WriteLine("[MenuItem(\"CONTEXT/Component/My Menu Item\")]");
// 			writer.WriteLine("private static void MyMenuItemFunction()"); 
// 			writer.BeginBlock();
// 			writer.WriteLine("Debug.Log(\"Hello World\");");
// 			writer.EndBlock();
// 			writer.EndBlock();
// 			writer.EndBlock();
// 			context.AddSource("MyMenuItem", SourceText.From(writer.ToString(), Encoding.UTF8));
// 		}
//
// 		public void Initialize(GeneratorInitializationContext context) { }
// 	}
// }