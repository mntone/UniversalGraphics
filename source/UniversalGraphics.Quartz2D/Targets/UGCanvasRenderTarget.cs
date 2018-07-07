using CoreGraphics;
using System;
using System.Diagnostics;

#if __MACOS__
using AppKit;
#else
using UIKit;
#endif

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGCanvasRenderTarget : IUGCanvasRenderTarget
	{
		object IUGObject.Native => Native;
		public CGImage Native
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

		private bool _disposed = false;
		private CGImage _native = null;

		public UGCanvasRenderTarget(UGSize canvasSize) : this(canvasSize, 1F) { }

		public UGCanvasRenderTarget(UGSize canvasSize, float scale)
		{
			Size = canvasSize;
			Scale = scale;
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			var native = _native;
			if (native != null)
			{
				native.Dispose();
				_native = null;
			}
			GC.SuppressFinalize(this);
		}

		public IUGContext CreateDrawingSession()
		{
			var bpp = 8;
			var width = (nint)(Scale * Width + .5F);
			var height = (nint)(Scale * Height + .5F);
			var stride = 4 * width;
			var canvasRect = new CGRect(0F, 0F, Width, Height);
			using (var colorSpace = CGColorSpace.CreateSrgb())
			{
				var context = new CGBitmapContext(null, width, height, bpp, stride, colorSpace, CGImageAlphaInfo.PremultipliedFirst);
				context.ScaleCTM(Scale, Scale);
				return new UGContext(context, canvasRect, Scale, () =>
				{
					_native = context.ToImage();
					context.Dispose();
					context = null;
				});
			}
		}

#if __MACOS__
		public NSImage GetImageAsNSImage()
		{
			var size = new CGSize(
				_native.Width,
				_native.Height);
			return new NSImage(Native, size);
		}
#else
		public UIImage GetImageAsUIImage() => new UIImage(Native);
#endif

		public float Width => Size.Width;
		public float Height => Size.Height;
		public UGSize Size { get; }
		public float Scale { get; }
	}
}
