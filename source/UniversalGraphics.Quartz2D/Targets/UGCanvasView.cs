#if !__WATCHOS__
using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System.ComponentModel;

#if __IOS__ || __TVOS__
using UIKit;
#elif __MACOS__
using AppKit;
#endif

namespace UniversalGraphics.Quartz2D
{
	[Register(nameof(UGCanvasViewBase))]
	[DesignTimeVisible(false)]
#if __IOS__ || __TVOS__
	public abstract class UGCanvasViewBase : UIView
#elif __MACOS__
	public abstract class UGCanvasViewBase : NSView
#endif
	{
		[Export("init")]
		public UGCanvasViewBase() : base() { }

		[DesignatedInitializer]
		[Export("initWithCoder:")]
		public UGCanvasViewBase(NSCoder coder) : base(coder) { }

		[Export("initWithFrame:")]
		public UGCanvasViewBase(CGRect frame) : base(frame) { }

		protected UGCanvasViewBase(NSObjectFlag t) : base(t) { }

		protected internal UGCanvasViewBase(IntPtr handle) : base(handle) { }

#if __IOS__ || __TVOS__
		public sealed override void Draw(CGRect rect)
		{
			base.Draw(rect);
			
			using (var context = UIGraphics.GetCurrentContext())
			using (var ugContext = new UGContext(context, rect, (float)ContentScaleFactor))
			{
				DrawOverride(ugContext);
			}
		}
#elif __MACOS__
		public sealed override void DrawRect(CGRect dirtyRect)
		{
			base.DrawRect(dirtyRect);

			CGContext context = null;
			try
			{
				context = NSGraphicsContext.CurrentContext.CGContext;
				context.SaveState();
				context.TranslateCTM(0F, dirtyRect.Height);
				context.ScaleCTM(1F, -1F);
				using (var ugContext = new UGContext(context, dirtyRect, (float)Window.BackingScaleFactor))
				{
					DrawOverride(ugContext);
				}
			}
			finally
			{
				if (context != null)
				{
					context.RestoreState();
					context.Dispose();
					context = null;
				}
			}
		}
#endif

		protected virtual void DrawOverride(IUGContext context)
		{
		}
	}

	[Register(nameof(UGCanvasView))]
	[DesignTimeVisible(true)]
	public class UGCanvasView : UGCanvasViewBase, IComponent
	{
		[Export("init")]
		public UGCanvasView() : base() => Initialize();

		public UGCanvasView(IUGCanvasViewDelegate @delegate) : this() => Delegate = @delegate;

		[DesignatedInitializer]
		[Export("initWithCoder:")]
		public UGCanvasView(NSCoder coder) : base(coder) { }

		[Export("initWithFrame:")]
		public UGCanvasView(CGRect frame) : base(frame) => Initialize();

		protected UGCanvasView(NSObjectFlag t) : base(t) => Initialize();

		protected internal UGCanvasView(IntPtr handle) : base(handle) { }

		public override void AwakeFromNib() => Initialize();

		protected virtual void Initialize()
		{
			IsDesignMode = ((IComponent)this).Site?.DesignMode ?? false;
		}

		protected override void DrawOverride(IUGContext context)
		{
			if (IsDesignMode)
				return;

			Delegate?.OnDraw(context);
		}

		private void OnInvalidating(object sender, EventArgs e) => SetNeedsDisplayInRect(Frame);

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

			SetNeedsDisplayInRect(Frame);
		}

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

		public bool IsDesignMode { get; private set; }

		#region IComponent implementation

		ISite IComponent.Site { get; set; }
		event EventHandler IComponent.Disposed
		{
			add => DisposedInternal += value;
			remove => DisposedInternal -= value;
		}

		private event EventHandler DisposedInternal;

		#endregion
	}
}
#endif
