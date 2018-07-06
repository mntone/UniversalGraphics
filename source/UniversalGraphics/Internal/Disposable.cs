using System;

namespace UniversalGraphics
{
	internal static class Disposable
	{
		private sealed class DisposableObject : IDisposable
		{
			public DisposableObject() { }

			public void Dispose() => GC.SuppressFinalize(this);
		}

		private sealed class ActionDisposableObject : IDisposable
		{
			private readonly Action _disposeAction;

			public ActionDisposableObject(Action disposeAction)
				=> _disposeAction = disposeAction;

			public void Dispose()
			{
				_disposeAction();
				GC.SuppressFinalize(this);
			}
		}

		public static IDisposable Create()
			=> new DisposableObject();

		public static IDisposable Create(Action disposeAction)
			=> new ActionDisposableObject(disposeAction);
	}
}
