using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UniversalGraphics
{
	public enum UGCapStyle : byte
	{
		Flat,
		Square,
		Round,
		Triangle,
	}

	public enum UGLineJoin : byte
	{
		Miter,
		Bevel,
		Round,
		MiterOrBevel,
	}

	public struct UGDashStyle
	{
		public enum Type : byte
		{
			Solid,
			Dash,
			Dot,
			DashDot,
			DashDotDot,
		}

		private float[] _Value;

		public UGDashStyle(float[] dashStyle)
		{
			_Value = dashStyle;
		}

		public UGDashStyle(Type type)
		{
			switch (type)
			{
				case Type.Solid:
					_Value = null;
					break;
				case Type.Dash:
					_Value = new float[] { 2, 2 };
					break;
				case Type.Dot:
					_Value = new float[] { 1, 1 };
					break;
				case Type.DashDot:
					_Value = new float[] { 2, 2, 1, 1 };
					break;
				case Type.DashDotDot:
					_Value = new float[] { 2, 2, 1, 1, 1, 1 };
					break;
				default:
					throw new ArgumentException(nameof(type));
			}
		}

		public float[] Value
		{
			get => _Value;
			set => _Value = value;
		}

		public static implicit operator float[](UGDashStyle d)
			=> d.Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct UGStrokeStyle
    {
		public UGCapStyle LineCap { get; set; }
		public UGLineJoin LineJoin { get; set; }
		public float MiterLimit { get; set; }
		public UGDashStyle DashStyle { get; set; }
		public float DashOffset { get; set; }
	}
}
