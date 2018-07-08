using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace UniversalGraphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct UGSize : IEquatable<UGSize>
	{
		public static readonly UGSize MaxValue = new UGSize(float.MaxValue);

		private Vector2 _data;

		public UGSize(float value) : this(value, value) { }

		public UGSize(float width, float height)
		{
			_data = new Vector2();
			_data.X = width;
			_data.Y = height;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is UGSize size))
			{
				return false;
			}

			return _data.Equals(size._data);
		}

		public bool Equals(UGSize other) => _data.Equals(other._data);

		public override int GetHashCode() => _data.GetHashCode();

		public float Width
		{
			get => _data.X;
			set => _data.X = value;
		}
		public float Height
		{
			get => _data.Y;
			set => _data.Y = value;
		}

		public static bool operator ==(UGSize a, UGSize b) => a._data == b._data;
		public static bool operator !=(UGSize a, UGSize b) => a._data != b._data;

		public static UGSize operator *(UGSize a, float b)
		{
			a._data *= b;
			return a;
		}
		public static UGSize operator *(float a, UGSize b)
		{
			b._data *= a;
			return b;
		}
		public static UGSize operator /(UGSize a, float b)
		{
			a._data /= b;
			return a;
		}
	}
}
