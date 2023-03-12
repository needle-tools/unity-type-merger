using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Mono.Cecil;
using Mono.Collections.Generic;
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

		public static void AddSafe<T>(this Collection<T> list, T member) where T : IMemberDefinition
		{
			for (var index = list.Count - 1; index >= 0; index--)
			{
				var existing = list[index];
				if (existing.Name == member.Name) list.RemoveAt(index);
			}
			list.Add(member);
		}

		public static FieldDefinition Copy(this FieldDefinition member)
		{
			var copy = new FieldDefinition(member.Name, member.Attributes, member.FieldType);
			copy.Constant = member.Constant;
			copy.InitialValue = member.InitialValue; 
			copy.IsRuntimeSpecialName = member.IsRuntimeSpecialName;
			copy.IsSpecialName = member.IsSpecialName;
			copy.IsStatic = member.IsStatic;
			copy.IsNotSerialized = member.IsNotSerialized;
			copy.IsPInvokeImpl = member.IsPInvokeImpl;
			return copy;
		}

		public static MethodDefinition Copy(this MethodDefinition member)
		{
			var copy = new MethodDefinition(member.Name, member.Attributes, member.ReturnType);
			copy.IsRuntimeSpecialName = member.IsRuntimeSpecialName;
			copy.IsSpecialName = member.IsSpecialName;
			copy.IsStatic = member.IsStatic;
			copy.IsPInvokeImpl = member.IsPInvokeImpl;
			copy.IsVirtual = member.IsVirtual;
			copy.IsNewSlot = member.IsNewSlot;
			copy.IsFinal = member.IsFinal;
			copy.IsHideBySig = member.IsHideBySig;
			copy.IsAbstract = member.IsAbstract;
			copy.IsAssembly = member.IsAssembly;
			copy.IsFamily = member.IsFamily;
			copy.IsFamilyAndAssembly = member.IsFamilyAndAssembly;
			copy.IsFamilyOrAssembly = member.IsFamilyOrAssembly;
			copy.IsPrivate = member.IsPrivate;
			copy.IsPublic = member.IsPublic;
			copy.IsReuseSlot = member.IsReuseSlot;
			foreach(var genericParams in member.GenericParameters)
				copy.GenericParameters.Add(genericParams);
			foreach(var par in member.Parameters)
				copy.Parameters.Add(par);
			copy.Body = member.Body;
			return copy;
		}
	}
}