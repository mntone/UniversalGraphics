using System;
using System.Windows.Forms;
using UniversalGraphics.GdiPlus.Win32;

namespace UniversalGraphics.GdiPlus
{
	public abstract class UGCanvasControlBase : Control
	{
		private ColorService _colorService;

		protected UGCanvasControlBase() : base() => Initialize();

		protected UGCanvasControlBase(string text) : base(text) => Initialize();

		protected UGCanvasControlBase(Control parent, string text) : base(parent, text) => Initialize();

		protected UGCanvasControlBase(string text, int left, int top, int width, int height) : base(text, left, top, width, height) => Initialize();

		protected UGCanvasControlBase(Control parent, string text, int left, int top, int width, int height) : base(parent, text, left, top, width, height) => Initialize();

		protected virtual void Initialize()
		{
			_colorService = new ColorService();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				if (_colorService != null)
				{
					_colorService.Dispose();
					_colorService = null;
				}
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			Invalidate(ClientRectangle);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var hMonitor = NativeMethods.GetHMonitor(ClientRectangle);
			_colorService.Initialize(hMonitor);

			var graphics = e.Graphics;
#if NET47 || NET471
			var scale = DeviceDpi / 96F;
			graphics.PageScale = scale;
			var scaledClientSize = new UGSize(
				ClientSize.Width / scale,
				ClientSize.Height / scale);
			using (var ugContext = new UGContext(graphics, scaledClientSize, DeviceDpi / 96F, _colorService))
#else
			var clientSize = new UGSize(
				ClientSize.Width,
				ClientSize.Height);
			using (var ugContext = new UGContext(graphics, clientSize, 1F, _colorService))
#endif
			{
				DrawOverride(ugContext);
			}
		}

		protected virtual void DrawOverride(IUGContext context)
		{
		}
	}

	public class UGCanvasControl : UGCanvasControlBase
	{
		public UGCanvasControl() : base() { }

		public UGCanvasControl(string text) : base(text) { }

		public UGCanvasControl(Control parent, string text) : base(parent, text) { }

		public UGCanvasControl(string text, int left, int top, int width, int height) : base(text, left, top, width, height) { }

		public UGCanvasControl(Control parent, string text, int left, int top, int width, int height) : base(parent, text, left, top, width, height) { }

		protected override void DrawOverride(IUGContext context)
		{
			if (DesignMode)
				return;

			Delegate?.OnDraw(context);
		}

		private void OnInvalidating(object sender, EventArgs e) => Invalidate(ClientRectangle);

		private void OnDelegateChanged(IUGCanvasViewDelegate oldDelegate)
		{
			if (oldDelegate is IUGAnimatableCanvasViewDelegate oldAnimatable)
			{
				oldAnimatable.Invalidating -= OnInvalidating;
			}

			if (Delegate is IUGAnimatableCanvasViewDelegate animatable)
			{
				animatable.Invalidating += OnInvalidating;
			}

			Invalidate(ClientRectangle);
		}

		public bool IsDesignMode => DesignMode;

		public IUGCanvasViewDelegate Delegate
		{
			get => _Delegate;
			set
			{
				if (_Delegate != value)
				{
					var oldDelegate = _Delegate;
					_Delegate = value;
					OnDelegateChanged(oldDelegate);
				}
			}
		}
		private IUGCanvasViewDelegate _Delegate;
	}
}
