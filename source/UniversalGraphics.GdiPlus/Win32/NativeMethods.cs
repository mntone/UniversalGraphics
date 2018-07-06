using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace UniversalGraphics.GdiPlus.Win32
{
	internal enum MonitorDefaultTo
	{
		Null = 0,
		Primary = 1,
		Nearest = 2,
	}

	internal enum MonitorInfoF : uint
	{
		Primary = 1
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct Profile
	{
		private const uint ProfileFilenameType = 1;
		private const uint ProfileMembufferType = 2;

		private const uint ProfileRead = 1;
		private const uint ProfileReadWrite = 2;

		public uint dwType;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string pProfileData;
		public uint cbDataSize;

		public Profile(string filename)
		{
			dwType = ProfileFilenameType;
			pProfileData = filename;
			cbDataSize = (uint)(filename.Length + 1) * 2u;
		}
	};

	internal enum ProfileAccess : uint
	{
		Read = 1,
		ReadWrite = 2,
	};

	internal enum FileShareMode : uint
	{
		Read = 1,
		Write = 2,
		Delete = 4
	};

	internal enum CreatationMode : uint
	{
		CreateNew = 1,
		CreateAlways = 2,
		OpenExisting = 3,
		OpenAlways = 4,
		TruncateExisting = 5,
	};

	internal enum ColorTransformMode : uint
	{
		Proof = 0x00000001,
		Normal = 0x00000002,
		Best = 0x00000003,

		EnableGamutChecking = 0x00010000,
		UseRelativeColorimetric = 0x00020000,
		FastTranslate = 0x00040000,
	}

	internal enum ColorType : int
	{
		Gray = 1,
		RGB,
		XYZ,
		Yxy,
		Lab,
		COLOR_3_CHANNEL,
		CMYK,
		COLOR_5_CHANNEL,
		COLOR_6_CHANNEL,
		COLOR_7_CHANNEL,
		COLOR_8_CHANNEL,
		COLOR_NAMED,
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct RGBColor
	{
		public ushort R;
		public ushort G;
		public ushort B;

		public RGBColor(ushort r, ushort g, ushort b)
		{
			R = r;
			G = g;
			B = b;
		}
	}

	internal static class NativeMethods
	{
		private const string USER32_DLL = "user32.dll";
		private const string GDI32_DLL = "gdi32.dll";
		private const string MSCMS_DLL = "mscms.dll";

		#region Window methods

		[StructLayout(LayoutKind.Sequential)]
		private struct NativeRect
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[DllImport(USER32_DLL)]
		private static extern IntPtr MonitorFromWindow([In] IntPtr hwnd, [In] MonitorDefaultTo dwFlags);

		[DllImport(USER32_DLL)]
		private static extern IntPtr MonitorFromRect([In] ref NativeRect lprc, [In] MonitorDefaultTo dwFlags);

		public static IntPtr GetHMonitor(this Rectangle rect, MonitorDefaultTo defaultTo = MonitorDefaultTo.Nearest)
		{
			var nativeRect = new NativeRect()
			{
				Left = rect.Left,
				Top = rect.Top,
				Right = rect.Right,
				Bottom = rect.Bottom,
			};
			return MonitorFromRect(ref nativeRect, defaultTo);
		}

		#endregion

		public static class ColorSpace
		{
			[DllImport(GDI32_DLL, EntryPoint = "CreateColorSpace")]
			private static extern IntPtr _CreateColorSpace([In, Out] ref LogColorSpace logColorSpace);

			public static IntPtr CreateColorSpace(ref LogColorSpace logColorSpace)
			{
				var hColorSpace = _CreateColorSpace(ref logColorSpace);
				if (hColorSpace == null)
				{
					throw new Win32Exception();
				}
				return hColorSpace;
			}

			[DllImport(GDI32_DLL, EntryPoint = "DeleteColorSpace")]
			private static extern bool _DeleteColorSpace(IntPtr hColorSpace);

			public static void DeleteColorSpace(IntPtr hColorSpace)
			{
				if (!_DeleteColorSpace(hColorSpace))
				{
					throw new Win32Exception();
				}
			}
		}

		public static class ColorProfile
		{
			[DllImport(MSCMS_DLL, EntryPoint = "OpenColorProfile", SetLastError = true, CharSet = CharSet.Unicode)]
			private static extern IntPtr _OpenColorProfile(
				[In] ref Profile pProfile,
				[In] ProfileAccess dwDesiredAccess,
				[In] FileShareMode dwShareMode,
				[In] CreatationMode dwCreationMode);

			public static IntPtr OpenColorProfile(
				ref Profile profile,
				ProfileAccess desiredAccess = ProfileAccess.Read,
				FileShareMode shareMode = FileShareMode.Read,
				CreatationMode creationMode = CreatationMode.OpenExisting)
			{
				var hProfile = _OpenColorProfile(ref profile, desiredAccess, shareMode, creationMode);
				if (hProfile == IntPtr.Zero)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				return hProfile;
			}
			
			[DllImport(MSCMS_DLL, EntryPoint = "CloseColorProfile", SetLastError = true)]
			private static extern bool _CloseColorProfile(
				[In] IntPtr hProfile);
			
			public static void CloseColorProfile(IntPtr hProfile)
			{
				if (!_CloseColorProfile(hProfile))
				{
					throw new Win32Exception();
				}
			}
		}

		public static class ColorTransform
		{
			[DllImport(MSCMS_DLL, EntryPoint = "CreateColorTransform", SetLastError = true)]
			private static extern IntPtr _CreateColorTransform(
				[In] ref LogColorSpace pLogColorSpace,
				[In] IntPtr hDestProfile,
				[In] IntPtr hTargetProfile,
				[In] ColorTransformMode dwFlags);

			internal static IntPtr CreateColorTransform(ref LogColorSpace colorSpace, IntPtr hDestProfile, IntPtr hTargetProfile, ColorTransformMode transformMode)
			{
				var hTransform = _CreateColorTransform(ref colorSpace, hDestProfile, hTargetProfile, transformMode);
				if (hTransform == IntPtr.Zero)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				return hTransform;
			}

			[DllImport(MSCMS_DLL, EntryPoint = "DeleteColorTransform", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool _DeleteColorTransform([In] IntPtr hTransform);

			internal static void DeleteColorTransform(IntPtr hTransform)
			{
				if (!_DeleteColorTransform(hTransform))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
			}

			[DllImport(MSCMS_DLL, EntryPoint = "TranslateColors", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool _TranslateColors(
				[In] IntPtr hTransform,
				[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] RGBColor[] paInputColors,
				uint nColors,
				ColorType ctInput,
				[In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] RGBColor[] paOutputColors,
				ColorType ctOutput);

			internal static RGBColor[] TranslateColors(IntPtr hTransform, RGBColor[] inputColors, ColorType inputType, ColorType outputType)
			{
				var length = inputColors.Length;
				var retValue = new RGBColor[length];
				if (!_TranslateColors(hTransform, inputColors, (uint)length, inputType, retValue, outputType))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				return retValue;
			}
		}

		#region Monitor methods

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct MonitorInfoEx
		{
			public int cbSize;
			public NativeRect rcMonitor;
			public NativeRect rcWork;
			public MonitorInfoF dwFlags;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szDevice;
		}

		[DllImport(USER32_DLL, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetMonitorInfo([In] IntPtr hMonitor, [In, Out] ref MonitorInfoEx info);

		private static MonitorInfoEx GetMonitorInfo(IntPtr hMonitor)
		{
			var info = new MonitorInfoEx()
			{
				cbSize = Marshal.SizeOf(typeof(MonitorInfoEx)),
			};
			if (!GetMonitorInfo(hMonitor, ref info))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return info;
		}

		#endregion

		#region ICC Profile methods

		[DllImport(GDI32_DLL, EntryPoint = "CreateDC", SetLastError = true)]
		private static extern IntPtr _CreateDC([In] string lpszDriver, [In] string lpszDevice, [In] string lpszOutput, [In] IntPtr lpInitData);

		private static IntPtr CreateDC(string driver, string device, string output, IntPtr initData)
		{
			var hDC = _CreateDC(driver, device, output, initData);
			if (hDC == IntPtr.Zero)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return hDC;
		}

		[DllImport(GDI32_DLL, EntryPoint = "DeleteDC", SetLastError = true)]
		private static extern bool _DeleteDC([In] IntPtr hDC);

		private static void DeleteDC(IntPtr hDC)
		{
			if (!_DeleteDC(hDC))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		[DllImport(GDI32_DLL, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetICMProfile([In] IntPtr hDC, [In, Out] ref uint lpcbName, [Out] StringBuilder lpszFilename);

		private static string GetICMProfile(this IntPtr hDC)
		{
			var length = 0u;
			if (!GetICMProfile(hDC, ref length, null) && length == 0)
			{
				throw new Win32Exception();
			}

			var builder = new StringBuilder((int)length);
			if (!GetICMProfile(hDC, ref length, builder))
			{
				throw new Win32Exception();
			}

			return builder.ToString();
		}

		internal static string GetICMProfileFromMonitor(IntPtr hMonitor)
		{
			var monitorInfo = GetMonitorInfo(hMonitor);
			var hDC = IntPtr.Zero;
			try
			{
				hDC = CreateDC(monitorInfo.szDevice, monitorInfo.szDevice, null, IntPtr.Zero);
				return hDC.GetICMProfile();
			}
			finally
			{
				if (hDC != IntPtr.Zero)
				{
					DeleteDC(hDC);
				}
			}
		}

		#endregion
	}
}
