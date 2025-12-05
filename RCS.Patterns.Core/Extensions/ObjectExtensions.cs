using OKB.Core.Types;
using OKB.Utilities;

namespace System;

public static class ObjectExtensions
{
	/// <summary>Formats the values as a tuple string with default braces</summary>
	public static string ToTupleString(params object[] values) =>
		new ValueFormatter().ToTupleString(values);

	/// <summary>Formats the values as a tuple string with the specified braces</summary>
	public static string ToTupleString(BraceType braceType, params object[] values) =>
		new ValueFormatter(braceType).ToTupleString(values);

	/// <summary>Formats the values as a tuple string where values are aligned within a specified width and default braces</summary>
	public static string ToTupleString(int valueWidth, Alignment alignment, params object[] values) =>
		new ValueFormatter(valueWidth, alignment).ToTupleString(values);

	/// <summary>Formats the values as a tuple string where values are aligned within a specified width and specified braces</summary>
	public static string ToTupleString(int valueWidth, Alignment alignment, BraceType braceType, params object[] values) =>
		new ValueFormatter(valueWidth, alignment, braceType).ToTupleString(values);
}

