using Microsoft.Graphics.Canvas;
using System;

#if WINDOWS_APP || WINDOWS_PHONE_APP
using Microsoft.Graphics.Canvas.Numerics;
#else
using System.Numerics;
#endif

namespace UniversalGraphics.Win2D
{
	internal sealed class CanvasLayer : IDisposable
	{
		public CanvasDrawingSession Session { get; }
		public CanvasActiveLayer Layer { get; }
		public Matrix3x2 Transform { get; }

		public CanvasLayer(CanvasDrawingSession session, CanvasActiveLayer layer, Matrix3x2 transform)
		{
			Session = session;
			Layer = layer;
			Transform = transform;
		}

		public void Dispose()
		{
			Layer.Dispose();
			Session.Transform = Transform;
			GC.SuppressFinalize(this);
		}
	}
}
