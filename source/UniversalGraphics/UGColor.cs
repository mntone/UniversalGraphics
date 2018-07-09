using System.Runtime.InteropServices;

namespace UniversalGraphics
{
	[StructLayout(LayoutKind.Explicit)]
	public struct UGColor
	{
		[FieldOffset(0)] public byte B;
		[FieldOffset(1)] public byte G;
		[FieldOffset(2)] public byte R;
		[FieldOffset(3)] public byte A;

		[FieldOffset(0)] public uint Color;
		[FieldOffset(0)] public int ColorAsInt;

		public UGColor(byte r, byte g, byte b)
			: this(255, r, g, b)
		{ }

		public UGColor(byte a, byte r, byte g, byte b)
		{
			Color = 0;
			ColorAsInt = 0;
			B = b;
			G = g;
			R = r;
			A = a;
		}

		public UGColor(byte a, UGColor color)
		{
			ColorAsInt = 0;
			B = 0;
			G = 0;
			R = 0;

			Color = color.Color;
			A = a;
		}

		private const float ONE_SIXTH = 1F / 6F;
		private const float ONE_THIRD = 1F / 3F;
		private const float TWO_THIRD = 2F / 3F;

		public static UGColor FromHSL(float h, float s, float l)
		{
			byte hueToRGB(float p, float q, float t)
			{
				if (t < 0F) t += 1F;
				if (t > 1F) t -= 1F;
				if (t < ONE_SIXTH) return (byte)(255F * (p + (q - p) * 6F * t));
				if (t < .5F) return (byte)(255F * q);
				if (t < TWO_THIRD) return (byte)(255F * (p + (q - p) * (TWO_THIRD - t) * 6F));
				return (byte)(255F * p);
			}

			byte r, g, b;
			if (s == 0F)
			{
				var l2 = (byte)(255F * l);
				r = l2;
				g = l2;
				b = l2;
			}
			else
			{
				var q = l < .5F ? l * (1F + s) : l + s - l * s;
				var p = 2 * l - q;
				r = hueToRGB(p, q, h + ONE_THIRD);
				g = hueToRGB(p, q, h);
				b = hueToRGB(p, q, h - ONE_THIRD);
			}
			return new UGColor(r, g, b);
		}
	}
}
