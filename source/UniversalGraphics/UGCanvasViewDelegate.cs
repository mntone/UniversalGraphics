using System;

namespace UniversalGraphics
{
	public interface IUGCanvasViewDelegate
	{
		void OnDraw(IUGContext context);
	}

	public interface IUGAnimatableCanvasViewDelegate : IUGCanvasViewDelegate
	{
		event EventHandler Invalidating;
	}

	public class UGCanvasViewDelegate : IUGCanvasViewDelegate
	{
		private bool _initialized = false;

		public virtual void InitializeResources(IUGContext context) { }

		public virtual void OnDraw(IUGContext context)
		{
			if (!_initialized)
			{
				InitializeResources(context);
				_initialized = true;
			}
		}
	}

	public class UGAnimatableCanvasViewDelegate : UGCanvasViewDelegate, IUGAnimatableCanvasViewDelegate
	{
		public void InvalidateVisual()
			=> Invalidating?.Invoke(this, EventArgs.Empty);

		public event EventHandler Invalidating;
	}
}
