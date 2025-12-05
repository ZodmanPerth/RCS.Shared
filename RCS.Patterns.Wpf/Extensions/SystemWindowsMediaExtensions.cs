using System;
using System.Globalization;
using System.Windows.Media;

namespace OKB.Patterns.Wpf.Extensions;

public static class SystemWindowsMediaExtensions
{
	/// <summary>Returns the ARGB hex of the brush</summary>
	public static string ToHexString(this SolidColorBrush brush)
	{
		Color colour = brush.Color;
		return $"#{colour.A:X2}{colour.R:X2}{colour.G:X2}{colour.B:X2}";
	}

	public static SolidColorBrush ToSolidColourBrush(this string? hex, SolidColorBrush? defaultBrush = null)
	{
		if (hex!.IsNullOrWhitespace())
			return GetDefault();

		hex = hex!
			.Trim()
			.TrimStart('#');

		// Expand RGB hex to AARRGGBB hex
		if (hex.Length == 3)
			hex = string.Concat("FF", hex[0], hex[0], hex[1], hex[1], hex[2], hex[2]);

		// Ensure alpha exists in hex string
		if (hex.Length == 6)
			hex = $"FF{hex}";

		// Check length is valid
		if (hex.Length != 8)
			return GetDefault();

		// Parse ARGB
		try
		{
			var a = byte.Parse(hex[0..2], NumberStyles.HexNumber);
			var r = byte.Parse(hex[2..4], NumberStyles.HexNumber);
			var g = byte.Parse(hex[4..6], NumberStyles.HexNumber);
			var b = byte.Parse(hex[6..8], NumberStyles.HexNumber);

			return new SolidColorBrush(Color.FromArgb(a, r, g, b));
		}
		catch
		{
			return GetDefault();
		}


		//// Local Functions


		SolidColorBrush GetDefault() =>
			defaultBrush ?? new SolidColorBrush(Colors.Transparent);
	}

}
