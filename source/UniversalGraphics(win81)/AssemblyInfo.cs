using System.Reflection;
using System.Runtime.InteropServices;

#if WINDOWS_APP
[assembly: AssemblyTitle("UniversalGraphics (win81)")]
#elif WINDOWS_PHONE_APP
[assembly: AssemblyTitle("UniversalGraphics (wpa81)")]
#endif
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("UniversalGraphics.Win2D")]
[assembly: AssemblyCopyright("Copyright © 2018 mntone")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.9.0.0")]
[assembly: ComVisible(false)]
