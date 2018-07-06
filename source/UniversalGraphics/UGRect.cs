using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace UniversalGraphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct UGRect : IEquatable<UGRect>
	{
		private Vector4 _data;

		public UGRect(float x, float y, float width, float height)
		{
			_data = new Vector4();
			_data.W = x;
			_data.X = y;
			_data.Y = width;
			_data.Z = height;
		}

		public UGRect(float x, float y, UGSize size)
		{
			_data = new Vector4();
			_data.W = x;
			_data.X = y;
			_data.Y = size.Width;
			_data.Z = size.Height;
		}

		public UGRect(Vector2 point, float width, float height)
		{
			_data = new Vector4();
			_data.W = point.X;
			_data.X = point.Y;
			_data.Y = width;
			_data.Z = height;
		}

		public UGRect(Vector2 point, UGSize size)
		{
			_data = new Vector4();
			_data.W = point.X;
			_data.X = point.Y;
			_data.Y = size.Width;
			_data.Z = size.Height;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is UGRect rect))
			{
				return false;
			}

			return _data.Equals(rect._data);
		}

		public bool Equals(UGRect other) => _data.Equals(other._data);

		public override int GetHashCode() => _data.GetHashCode();

		public float X
		{
			get => _data.W;
			set => _data.W = value;
		}
		public float Y
		{
			get => _data.X;
			set => _data.X = value;
		}
		public float Width
		{
			get => _data.Y;
			set => _data.Y = value;
		}
		public float Height
		{
			get => _data.Z;
			set => _data.Z = value;
		}

		public float Left
		{
			get => X;
			set => X = value;
		}
		public float Top
		{
			get => Y;
			set => Y = value;
		}
		public float Right
		{
			get => X + Width;
			set => Width = value - X;
		}
		public float Bottom
		{
			get => Y + Height;
			set => Height = value - Y;
		}

		public Vector2 TopLeft
		{
			get => new Vector2(X, Y);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}
		public Vector2 TopRight
		{
			get => new Vector2(X + Width, Y);
			set
			{
				Width = value.X - X;
				Y = value.Y;
			}
		}
		public Vector2 BottomLeft
		{
			get => new Vector2(X, Y);
			set
			{
				X = value.X;
				Height = value.Y - Y;
			}
		}
		public Vector2 BottomRight
		{
			get => new Vector2(X, Y + Height);
			set
			{
				Width = value.X - X;
				Height = value.Y - Y;
			}
		}

		public UGSize Size
		{
			get => new UGSize(Width, Height);
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		public static bool operator ==(UGRect a, UGRect b) => a._data == b._data;
		public static bool operator !=(UGRect a, UGRect b) => a._data != b._data;

		public static UGRect operator +(UGRect a, Vector2 b)
		{
			a.X += b.X;
			a.Y += b.Y;
			return a;
		}
		public static UGRect operator +(Vector2 a, UGRect b)
		{
			b.X += a.X;
			b.Y += a.Y;
			return b;
		}
		public static UGRect operator -(UGRect a, Vector2 b)
		{
			a.X -= b.X;
			a.Y -= b.Y;
			return a;
		}
		public static UGRect operator *(UGRect a, float b)
		{
			a._data *= b;
			return a;
		}
		public static UGRect operator *(float a, UGRect b)
		{
			b._data *= a;
			return b;
		}
		public static UGRect operator /(UGRect a, float b)
		{
			a._data /= b;
			return a;
		}
	}
}
