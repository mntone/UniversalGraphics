using System;

namespace UniversalGraphics
{
	public struct UGGradientStop : IEquatable<UGGradientStop>
	{
		private UGColor _color;
		private float _offset;

		public UGGradientStop(UGColor color, float offset)
		{
			_color = color;
			_offset = offset;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is UGGradientStop gradientStop))
			{
				return false;
			}

			return _color.Equals(gradientStop._color)
				&& _offset.Equals(gradientStop._offset);
		}

		public bool Equals(UGGradientStop other)
			=> _color.Equals(other._color)
			&& _offset.Equals(other._offset);

		public override int GetHashCode()
			=> _color.GetHashCode() ^ _offset.GetHashCode();

		public UGColor Color
		{
			get => _color;
			set => _color = value;
		}
		public float Offset
		{
			get => _offset;
			set => _offset = value;
		}
	}
}
