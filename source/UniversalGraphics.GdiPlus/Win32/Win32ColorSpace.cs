using System;

namespace UniversalGraphics.GdiPlus.Win32
{
	public sealed class Win32ColorSpace
	{
		internal LogColorSpace _colorSpaceInfo;

		public Win32ColorSpace() : this(LogColorSpace.CreateSRGB()) { }

		private Win32ColorSpace(LogColorSpace colorSpace)
		{
			_colorSpaceInfo = colorSpace;
		}

		private void InvalidateColorSpace() { }

		public GamutMappingIntent Intent
		{
			get => _colorSpaceInfo.lcsIntent;
			set
			{
				if (_colorSpaceInfo.lcsIntent != value)
				{
					_colorSpaceInfo.lcsIntent = value;
					InvalidateColorSpace();
				}
			}
		}

		public float GammaRed
		{
			get
			{
				if (IsSRGB) return 2.2F;
				if (IsWindowsColorSpace) throw new NotSupportedException();
				return Utils.ConvertFloatingPoint_Non_8_8_Non(_colorSpaceInfo.lcsGammaRed);
			}
			set
			{
				if (!IsCalibratedRGB) throw new InvalidOperationException();

				var value2 = Utils.ConvertFixedPoint_Non_8_8_Non(value);
				if (_colorSpaceInfo.lcsGammaRed != value2)
				{
					_colorSpaceInfo.lcsGammaRed = value2;
					InvalidateColorSpace();
				}
			}
		}

		public float GammaGreen
		{
			get
			{
				if (IsSRGB) return 2.2F;
				if (IsWindowsColorSpace) throw new NotSupportedException();
				return Utils.ConvertFloatingPoint_Non_8_8_Non(_colorSpaceInfo.lcsGammaGreen);
			}
			set
			{
				if (!IsCalibratedRGB) throw new InvalidOperationException();

				var value2 = Utils.ConvertFixedPoint_Non_8_8_Non(value);
				if (_colorSpaceInfo.lcsGammaGreen != value2)
				{
					_colorSpaceInfo.lcsGammaGreen = value2;
					InvalidateColorSpace();
				}
			}
		}

		public float GammaBlue
		{
			get
			{
				if (IsSRGB) return 2.2F;
				if (IsWindowsColorSpace) throw new NotSupportedException();
				return Utils.ConvertFloatingPoint_Non_8_8_Non(_colorSpaceInfo.lcsGammaBlue);
			}
			set
			{
				if (!IsCalibratedRGB) throw new InvalidOperationException();

				var value2 = Utils.ConvertFixedPoint_Non_8_8_Non(value);
				if (_colorSpaceInfo.lcsGammaBlue != value2)
				{
					_colorSpaceInfo.lcsGammaBlue = value2;
					InvalidateColorSpace();
				}
			}
		}

		public bool IsSRGB => _colorSpaceInfo.lcsCSType == LogicalColorSpace.sRGB;
		public bool IsCalibratedRGB => _colorSpaceInfo.lcsCSType == LogicalColorSpace.CalibratedRGB;
		public bool IsWindowsColorSpace => _colorSpaceInfo.lcsCSType == LogicalColorSpace.WindowsColorSpace;

		public static Win32ColorSpace CreateSRGB() => new Win32ColorSpace();

		public static Win32ColorSpace CreateFromHMonitor(IntPtr hMonitor)
		{
			var profile = NativeMethods.GetICMProfileFromMonitor(hMonitor);
			var logColorSpace = LogColorSpace.CreateFromICCProfile(profile);
			return new Win32ColorSpace(logColorSpace);
		}

		internal static class Utils
		{
			private const uint BASE_NON_8_8_NON = 1 << 8;

			public static float ConvertFloatingPoint_Non_8_8_Non(uint fixedPoint)
			{
				float retValue = (fixedPoint >> 16) & 0xff;
				fixedPoint >>= 8;

				var mask = BASE_NON_8_8_NON;
				var digit = .5F;
				for (var i = 0; i < 8; ++i)
				{
					if ((fixedPoint & mask) != 0u)
					{
						retValue += digit;
					}
					mask >>= 1;
					digit /= 2F;
				}
				return retValue;
			}

			public static uint ConvertFixedPoint_Non_8_8_Non(float floatingPoint)
				=> (uint)(BASE_NON_8_8_NON * floatingPoint) << 8;
		}
	}
}
