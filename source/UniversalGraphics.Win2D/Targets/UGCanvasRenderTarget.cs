using Microsoft.Graphics.Canvas;
using System;
using System.Diagnostics;
using Windows.Foundation;

namespace UniversalGraphics.Win2D
{
	public sealed class UGCanvasRenderTarget : IUGCanvasRenderTarget
	{
		object IUGObject.Native => Native;
		public CanvasRenderTarget Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return _native;
			}
		}

		private readonly CanvasDevice _device;
		private readonly CanvasRenderTarget _native;

		private bool _disposed = false;

		public UGCanvasRenderTarget(UGSize canvasSize) : this(canvasSize, 1F) { }

		public UGCanvasRenderTarget(UGSize canvasSize, float scale)
		{
			Size = canvasSize;
			Scale = scale;

			_device = CanvasDevice.GetSharedDevice();
			_native = new CanvasRenderTarget(_device, Width, Height, Scale * 96.0F);
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native?.Dispose();
			GC.SuppressFinalize(this);
		}

		public IUGContext CreateDrawingSession()
		{
			var canvasSize = new Size(Width, Height);
			var session = Native.CreateDrawingSession();
			return new UGContext(_device, session, canvasSize, () =>
			{
				session.Dispose();
				session = null;
			});
		}

		public float Width => Size.Width;
		public float Height => Size.Height;
		public UGSize Size { get; }
		public float Scale { get; }
	}
}
