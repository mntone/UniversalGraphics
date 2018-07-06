using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGCanvasRenderTarget : IUGCanvasRenderTarget
	{
		object IUGObject.Native => Native;
		public Bitmap Native
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

		private readonly Bitmap _native;

		private bool _disposed = false;

		public UGCanvasRenderTarget(UGSize canvasSize) : this(canvasSize, 1F) { }

		public UGCanvasRenderTarget(UGSize canvasSize, float scale)
		{
			Size = canvasSize;
			Scale = scale;

			var width = (int)(Scale * Size.Width + .5F);
			var height = (int)(Scale * Size.Height + .5F);
			var dpi = (int)(Scale * 96F + .5F);
			var native = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
			native.SetResolution(dpi, dpi);
			_native = native;
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public IUGContext CreateDrawingSession()
		{
			var context = Graphics.FromImage(Native);
			context.PageScale = Scale;
			context.PageUnit = GraphicsUnit.Pixel;
			var colorService = new ColorService();
			return new UGContext(context, Size, Scale, colorService, () =>
			{
				colorService.Dispose();
				context.Dispose();
				context = null;
			});
		}

		public float Width => Size.Width;
		public float Height => Size.Height;
		public UGSize Size { get; }
		public float Scale { get; }
	}
}
