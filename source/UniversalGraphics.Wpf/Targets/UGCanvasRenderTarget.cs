using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UniversalGraphics.Wpf
{
	public sealed class UGCanvasRenderTarget : IUGCanvasRenderTarget
	{
		object IUGObject.Native => Native;
		public RenderTargetBitmap Native
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

		private readonly RenderTargetBitmap _native;

		private bool _disposed = false;

		public UGCanvasRenderTarget(UGSize canvasSize) : this(canvasSize, 1F) { }

		public UGCanvasRenderTarget(UGSize canvasSize, float scale)
		{
			Size = canvasSize;
			Scale = scale;

			var width = (int)(Scale * Width + .5F);
			var height = (int)(Scale * Height + .5F);
			var dpi = 96F * Scale;
			_native = new RenderTargetBitmap(width, height, dpi, dpi, PixelFormats.Default);
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}
		
		public IUGContext CreateDrawingSession()
		{
			var canvasSize = new Size(Width, Height);
			var drawingVisual = new DrawingVisual();
			var context = drawingVisual.RenderOpen();
			return new UGContext(drawingVisual, context, canvasSize, Scale, () =>
			{
				context.Close();
				Native.Render(drawingVisual);
				((IDisposable)context).Dispose();
				context = null;
			});
		}

		public float Width => Size.Width;
		public float Height => Size.Height;
		public UGSize Size { get; }
		public float Scale { get; }
	}
}
