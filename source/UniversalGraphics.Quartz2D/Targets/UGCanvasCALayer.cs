#if !__WATCHOS__
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System;

namespace UniversalGraphics.Quartz2D
{
	[Register(nameof(UGCanvasCALayerBase))]
	public abstract class UGCanvasCALayerBase : CALayer
	{
		[Export("init")]
		public UGCanvasCALayerBase()
		{
			SetNeedsDisplay();
			NeedsDisplayOnBoundsChange = true;
		}

		[Export("initWithLayer:")]
		public UGCanvasCALayerBase(CALayer other) : base(other)
		{
			SetNeedsDisplay();
			NeedsDisplayOnBoundsChange = true;
		}

		[DesignatedInitializer]
		[Export("initWithCoder:")]
		public UGCanvasCALayerBase(NSCoder coder) : base(coder)
		{
			SetNeedsDisplay();
			NeedsDisplayOnBoundsChange = true;
		}

		protected UGCanvasCALayerBase(NSObjectFlag t) : base(t)
		{
			SetNeedsDisplay();
			NeedsDisplayOnBoundsChange = true;
		}

		protected internal UGCanvasCALayerBase(IntPtr handle) : base(handle)
		{
			SetNeedsDisplay();
			NeedsDisplayOnBoundsChange = true;
		}

		public sealed override void DrawInContext(CGContext context)
		{
			base.DrawInContext(context);
			
			using (var ugContext = new UGContext(context, Frame, (float)ContentsScale))
			{
				DrawOverride(ugContext);
			}
		}

		protected virtual void DrawOverride(IUGContext context)
		{
		}
	}

	[Register(nameof(UGCanvasCALayer))]
	public class UGCanvasCALayer : UGCanvasCALayerBase
	{
		public IUGCanvasViewDelegate CanvasDelegate { get; set; }

		[Export("init")]
		public UGCanvasCALayer() : base() { }

		public UGCanvasCALayer(IUGCanvasViewDelegate @delegate) => CanvasDelegate = @delegate;

		[Export("initWithLayer:")]
		public UGCanvasCALayer(CALayer other) : base(other) { }

		[Export("initWithCoder:")]
		public UGCanvasCALayer(NSCoder coder) : base(coder) { }

		protected UGCanvasCALayer(NSObjectFlag t) : base(t) { }

		protected internal UGCanvasCALayer(IntPtr handle) : base(handle) { }

		protected override void DrawOverride(IUGContext context) => CanvasDelegate?.OnDraw(context);
	}
}
#endif
