using System;
using System.Collections.Generic;
using System.Drawing;
using UniversalGraphics.GdiPlus.Win32;

namespace UniversalGraphics.GdiPlus
{
	internal sealed class ColorService : IDisposable
	{
		public IntPtr CurrentHMonitor { get; private set; }

		private readonly Win32ColorSpace _csForColor = Win32ColorSpace.CreateSRGB();
		private readonly Dictionary<uint, Color> _translatedColors = new Dictionary<uint, Color>();

		private IntPtr _hTransform = IntPtr.Zero;
		
		private void Reset()
		{
			_translatedColors.Clear();
			_hTransform = IntPtr.Zero;
		}

		internal void Initialize(IntPtr hMonitor)
		{
			if (CurrentHMonitor != hMonitor)
			{
				CurrentHMonitor = hMonitor;
				Reset();

				var profilePath = NativeMethods.GetICMProfileFromMonitor(CurrentHMonitor);
				var profile = new Profile(profilePath);
				var hProfile = IntPtr.Zero;
				try
				{
					hProfile = NativeMethods.ColorProfile.OpenColorProfile(ref profile);

					_hTransform = NativeMethods.ColorTransform.CreateColorTransform(ref _csForColor._colorSpaceInfo, hProfile, IntPtr.Zero, ColorTransformMode.Normal | ColorTransformMode.EnableGamutChecking);
				}
				finally
				{
					if (hProfile != IntPtr.Zero)
					{
						NativeMethods.ColorProfile.CloseColorProfile(hProfile);
					}
				}
			}
		}

		public void Dispose()
		{
			var hTransform = _hTransform;
			if (hTransform != IntPtr.Zero)
			{
				NativeMethods.ColorTransform.DeleteColorTransform(hTransform);
			}
			GC.SuppressFinalize(this);
		}

		public Color GetTranslatedColor(UGColor color)
		{
			var hTransform = _hTransform;
			if (hTransform == IntPtr.Zero) return color.ToGDIColor();

			var value = color.Color;
			if (!_translatedColors.TryGetValue(value, out var translatedColor))
			{
				var colors = new RGBColor[1];
				colors[0].R = (ushort)(257 * color.R);
				colors[0].G = (ushort)(257 * color.G);
				colors[0].B = (ushort)(257 * color.B);

				var buf = NativeMethods.ColorTransform.TranslateColors(hTransform, colors, ColorType.RGB, ColorType.RGB);
				translatedColor = Color.FromArgb(color.A, (byte)(buf[0].R / 257), (byte)(buf[0].G / 257), (byte)(buf[0].B / 257));
				_translatedColors.Add(value, translatedColor);
			}
			return translatedColor;
		}
	}
}
