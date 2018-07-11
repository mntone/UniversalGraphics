using System;
using System.Linq;
using System.Reflection;

namespace UniversalGraphics.Test.Infrastructures
{
	public static class AssemblyHelper
	{
#if WINDOWS_APP || WINDOWS_PHONE_APP
		public static Type[] GetInterfacesFromCurrentAssembly<T>()
			=> typeof(AssemblyHelper).GetTypeInfo().Assembly.GetInterfaces<T>();
#else
		public static Type[] GetInterfacesFromCurrentAssembly<T>()
			=> Assembly.GetExecutingAssembly().GetInterfaces<T>();
#endif
	}

	public static class AssemblyExtensions
    {
		public static Type[] GetInterfaces<T>(this Assembly assembly)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			return assembly.DefinedTypes
				.Where(c => c.ImplementedInterfaces.Any(t => t == typeof(T)))
				.Select(info => info.AsType())
				.ToArray();
#else
			return assembly.GetTypes()
				.Where(c => c.GetInterfaces().Any(t => t == typeof(T)))
				.ToArray();
#endif
		}
	}
}
