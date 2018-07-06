using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
[assembly: AssemblyTitle("UniversalGraphics.Test (Win2D)")]
#elif WINDOWS_WPF
[assembly: AssemblyTitle("UniversalGraphics.Test (WPF)")]
#elif __ANDROID__
[assembly: AssemblyTitle("UniversalGraphics.Test (Droid2D)")]
#elif __MACOS__ || __IOS__ || __TVOS__ || __WATCHOS__
[assembly: AssemblyTitle("UniversalGraphics.Test (Quartz2D)")]
#else
[assembly: AssemblyTitle("UniversalGraphics.Test (GDI+)")]
#endif
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("UniversalGraphics.Test")]
[assembly: AssemblyCopyright("Copyright © 2018 mntone")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if WINDOWS_WPF
[assembly: ThemeInfo(
	ResourceDictionaryLocation.None,
	ResourceDictionaryLocation.SourceAssembly)]
#endif

[assembly: AssemblyVersion("0.9.*")]
[assembly: ComVisible(false)]
[assembly: Guid("e20355f8-7891-4dda-903e-cbbf772766cd")]
