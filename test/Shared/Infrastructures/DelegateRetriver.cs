using System;

namespace UniversalGraphics.Test.Infrastructures
{
	public static class DelegateRetriver
    {
		private static Type[] _delegateTypes;

		public static Type[] GetDelegateTypes()
			=> _delegateTypes ?? (_delegateTypes = AssemblyHelper.GetInterfacesFromCurrentAssembly<IUGCanvasViewDelegate>());

		public static IUGCanvasViewDelegate CreateDelegate(Type type)
			=> (IUGCanvasViewDelegate)Activator.CreateInstance(type);
	}
}
