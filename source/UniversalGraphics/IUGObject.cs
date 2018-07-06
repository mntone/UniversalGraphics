using System;

namespace UniversalGraphics
{
	public interface IUGObject : IDisposable
    {
		object Native { get; }
    }
}
