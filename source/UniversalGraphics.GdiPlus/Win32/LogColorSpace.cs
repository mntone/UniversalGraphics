using System.Runtime.InteropServices;

namespace UniversalGraphics.GdiPlus.Win32
{
	internal enum LogicalColorSpace : int
	{
		CalibratedRGB = 0x00000000,
		sRGB = 0x73524742, // 'sRGB'
		WindowsColorSpace = 0x57696E20, // 'Win '
	}

	public enum GamutMappingIntent : uint
	{
		// LCS_GM_BUSINESS
		Saturation = 0x00000001,
		// LCS_GM_GRAPHICS
		Relative = 0x00000002,
		// LCS_GM_IMAGES
		Perceptual = 0x00000004,
		// LCS_GM_ABS_COLORIMETRIC
		Absolute = 0x00000008,
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct CieXyz
	{
		public int X;
		public int Y;
		public int Z;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct CieXyzTriple
	{
		public CieXyz CieXyzRed;
		public CieXyz CieXyzGreen;
		public CieXyz CieXyzBlue;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct LogColorSpace
	{
		private const uint LCS_SIGNATURE = 0x50534F43; // 'PSOC'

		private uint lcsSignature;
		private uint lcsVersinon;
		private uint lcsSize;
		public LogicalColorSpace lcsCSType;
		public GamutMappingIntent lcsIntent;
		public CieXyzTriple lcsEndpoints;
		public uint lcsGammaRed;
		public uint lcsGammaGreen;
		public uint lcsGammaBlue;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		private string lcsFilename;

		private static LogColorSpace Init()
		{
			return new LogColorSpace()
			{
				lcsSignature = LCS_SIGNATURE,
				lcsVersinon = 0x00000400,
				lcsSize = (uint)Marshal.SizeOf(typeof(LogColorSpace)),
				lcsCSType = LogicalColorSpace.CalibratedRGB,
				lcsIntent = GamutMappingIntent.Perceptual,
				lcsEndpoints = new CieXyzTriple(),
			};
		}

		public static LogColorSpace CreateSRGB()
		{
			var retValue = Init();
			retValue.lcsCSType = LogicalColorSpace.sRGB;
			return retValue;
		}

		public static LogColorSpace CreateFromICCProfile(string filename)
		{
			var retValue = Init();
			retValue.lcsFilename = filename;
			return retValue;
		}
	}
}
