using System;
using System.Linq;

namespace UniversalGraphics.Quartz2D
{
	internal static unsafe class CGFunctionsHelper
	{
		private const float EPSILON = 0.0001F;

		public delegate void CGFunctionEvaluateWithStops(UGGradientStop[] stops, nfloat* data, nfloat* outData);

		public static CGFunctionEvaluateWithStops GetCGFunction(UGEdgeBehavior edgeBehavior)
		{
			switch (edgeBehavior)
			{
			case UGEdgeBehavior.Clamp:
				return CGClampFunction;

			case UGEdgeBehavior.Wrap:
				return CGWrapFunction;

			case UGEdgeBehavior.Mirror:
				return CGMirrorFunction;

			default:
				throw new NotSupportedException();
			}
		}

		private static void CGClampFunction(UGGradientStop[] stops, nfloat* data, nfloat* outData)
		{
			var pos = *data;
			CGFunctionBase(stops, pos, outData);
		}

		private static void CGWrapFunction(UGGradientStop[] stops, nfloat* data, nfloat* outData)
		{
			var pos = *data % 1;
			CGFunctionBase(stops, pos, outData);
		}

		private static void CGMirrorFunction(UGGradientStop[] stops, nfloat* data, nfloat* outData)
		{
			var pos = NMath.Abs(NMath.IEEERemainder(*data, 2));
			CGFunctionBase(stops, pos, outData);
		}

		private static void CGFunctionBase(UGGradientStop[] stops, nfloat pos, nfloat* outData)
		{
			var first = stops.First();
			if (pos <= first.Offset + EPSILON)
			{
				SetColor(first.Color, outData);
				return;
			}

			var last = stops.Last();
			if (pos >= last.Offset - EPSILON)
			{
				SetColor(last.Color, outData);
				return;
			}

			var e = stops.GetEnumerator();
			e.MoveNext();
			var from = (UGGradientStop)e.Current;
			while (e.MoveNext())
			{
				var to = (UGGradientStop)e.Current;
				if (pos > from.Offset && pos <= to.Offset + EPSILON)
				{
					var ratio = (pos - from.Offset) / (to.Offset - from.Offset);
					SetMixedColor(ratio, from.Color, to.Color, outData);
					return;
				}

				from = to;
			}
			//throw new InvalidOperationException();
		}

		private static unsafe void SetColor(UGColor inColor, nfloat* outColor)
		{
			outColor[0] = (nfloat)inColor.R / 255;
			outColor[1] = (nfloat)inColor.G / 255;
			outColor[2] = (nfloat)inColor.B / 255;
			outColor[3] = (nfloat)inColor.A / 255;
		}

		private static unsafe void SetMixedColor(nfloat ratio, UGColor inFromColor, UGColor inToColor, nfloat* outColor)
		{
			outColor[0] = (inFromColor.R + ratio * (inToColor.R - inFromColor.R)) / 255;
			outColor[1] = (inFromColor.G + ratio * (inToColor.G - inFromColor.G)) / 255;
			outColor[2] = (inFromColor.B + ratio * (inToColor.B - inFromColor.B)) / 255;
			outColor[3] = (inFromColor.A + ratio * (inToColor.A - inFromColor.A)) / 255;
		}
	}
}
