using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalGraphics.Win2D
{
	[TemplatePart(Name = CANVAS_CONTROL_NAME, Type = typeof(CanvasControl))]
	public abstract class UGCanvasControlBase : Control
	{
		protected const string CANVAS_CONTROL_NAME = "CanvasControl";

		private CanvasControl _canvasControl;

		public UGCanvasControlBase()
			: base()
		{
			DefaultStyleKey = typeof(UGCanvasControlBase);

			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			Debug.Assert(_canvasControl == null);
			_canvasControl = (CanvasControl)GetTemplateChild(CANVAS_CONTROL_NAME);
			_canvasControl.Draw += OnDraw;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoaded;

			_canvasControl?.Invalidate();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			Unloaded -= OnUnloaded;

			Debug.Assert(_canvasControl != null);
			_canvasControl.Draw -= OnDraw;
			_canvasControl.RemoveFromVisualTree();
			_canvasControl = null;
		}

		private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
		{
			using (var ugContext = new UGContext(_canvasControl.Device, args.DrawingSession, RenderSize))
			{
				DrawOverride(ugContext);
			}
		}

		protected virtual void DrawOverride(IUGContext context) { }

		protected void InvalidateVisual() => _canvasControl?.Invalidate();
	}

	[TemplatePart(Name = CANVAS_CONTROL_NAME, Type = typeof(CanvasControl))]
	public class UGCanvasControl : UGCanvasControlBase
	{
		public UGCanvasControl()
			: base()
		{
			IsDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
		}

		protected override void DrawOverride(IUGContext context)
		{
			if (IsDesignMode)
				return;

			Delegate?.OnDraw(context);
		}

		private void OnInvalidating(object sender, EventArgs e) => InvalidateVisual();

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

			InvalidateVisual();
		}

		public bool IsDesignMode { get; private set; }

		public IUGCanvasViewDelegate Delegate
		{
			get => (IUGCanvasViewDelegate)GetValue(DelegateProperty);
			set => SetValue(DelegateProperty, value);
		}

		public static readonly DependencyProperty DelegateProperty
#if WINDOWS_PHONE_APP
			= DependencyProperty.Register(nameof(Delegate), typeof(object), typeof(UGCanvasControl), new PropertyMetadata(null, OnDelegateChanged));
#else
			= DependencyProperty.Register(nameof(Delegate), typeof(IUGCanvasViewDelegate), typeof(UGCanvasControl), new PropertyMetadata(null, OnDelegateChanged));
#endif

		private static void OnDelegateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var canvasElement = (UGCanvasControl)d;
			canvasElement.OnDelegateChanged((IUGCanvasViewDelegate)e.OldValue);
		}
	}
}
