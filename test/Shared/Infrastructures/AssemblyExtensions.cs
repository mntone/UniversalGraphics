using System;
using System.Linq;
using System.Reflection;

namespace UniversalGraphics.Test.Infrastructures
{
	public static class AssemblyHelper
	{
		public static Type[] GetInterfacesFromCurrentAssembly<T>()
			=> Assembly.GetExecutingAssembly().GetInterfaces<T>();
	}

    public static class AssemblyExtensions
    {
		public static Type[] GetInterfaces<T>(this Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(c => c.GetInterfaces().Any(t => t == typeof(T)))
				.ToArray();
		}
	}
}
