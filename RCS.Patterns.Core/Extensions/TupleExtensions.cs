using OKB.Core.Types;
using OKB.Utilities;

#nullable enable

namespace System;

public static class TupleExtensions
{
	static Dictionary<string, ValueFormatter> _valueFormatterCache = new();         // Cache of same-configuration value formatters using a generated key
	static Dictionary<string, ValueFormatter> _valueOfCountFormatterCache = new();  // Cache of same-configuration value of type formatters using a generated key



	//// Helpers


	/// <summary>Returns a value formatter with the passed configuration.  Caches value formatters with the same configuration for reuse.</summary>
	static ValueFormatter GetCachedValueFormatter(BraceType braceType = BraceType.Round)
	{
		var key = $"n,Right,{braceType}";

		if (_valueFormatterCache.TryGetValue(key, out var formatter))
			return formatter;

		formatter = new ValueFormatter(braceType);
		_valueFormatterCache[key] = formatter;
		return formatter;
	}

	/// <summary>Returns a value formatter with the passed configuration.  Caches value formatters with the same configuration for reuse.</summary>
	static ValueFormatter GetCachedValueFormatter(int valueWidth, Alignment alignment = Alignment.Right, BraceType braceType = BraceType.Round)
	{
		string key = $"{valueWidth},{alignment},{braceType}";

		if (_valueFormatterCache.TryGetValue(key, out var formatter))
			return formatter;

		formatter = new ValueFormatter(valueWidth, alignment, braceType);
		_valueFormatterCache[key] = formatter;
		return formatter;
	}

	/// <summary>Returns a value formatter for value of count calls with the passed configuration.  Caches value formatters with the same configuration for reuse.</summary>
	static ValueFormatter GetCachedValueOfCountFormatter(int valueWidth, Alignment alignment = Alignment.Right, BraceType braceType = BraceType.Round)
	{
		string key = $"{valueWidth},{alignment},{braceType}";

		if (_valueOfCountFormatterCache.TryGetValue(key, out var formatter))
			return formatter;

		formatter = new ValueFormatter(valueWidth, alignment, braceType).WithDelimiter("/");
		_valueOfCountFormatterCache[key] = formatter;
		return formatter;
	}



	//// single value


	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString(this object value) =>
		GetCachedValueFormatter().ToTupleString(value);

	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString(this object value, BraceType braceType) =>
		GetCachedValueFormatter(braceType).ToTupleString(value);

	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString(this object value, int valueWidth, Alignment alignment) =>
		GetCachedValueFormatter(valueWidth, alignment).ToTupleString(value);

	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString(this object value, int valueWidth, Alignment alignment, BraceType braceType) =>
		GetCachedValueFormatter(valueWidth, alignment, braceType).ToTupleString(value);



	//// Tuple with 1 item


	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString<T>(this ValueTuple<T> valueTuple) =>
		GetCachedValueFormatter().ToTupleString(valueTuple.Item1);

	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString<T>(this ValueTuple<T> valueTuple, BraceType braceType) =>
		GetCachedValueFormatter(braceType).ToTupleString(valueTuple.Item1);

	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString<T>(this ValueTuple<T> valueTuple, int valueWidth, Alignment alignment) =>
		GetCachedValueFormatter(valueWidth, alignment).ToTupleString(valueTuple.Item1);

	/// <summary>Returns a string such as "( 1)" with optional braces</summary>
	public static string ToTupleString<T>(this ValueTuple<T> valueTuple, int valueWidth, Alignment alignment, BraceType braceType) =>
		GetCachedValueFormatter(valueWidth, alignment, braceType).ToTupleString(valueTuple.Item1);



	//// Tuple with 2 items


	/// <summary>Returns a string such as "( 1, 5)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2) valueTuple) =>
		GetCachedValueFormatter().ToTupleString(valueTuple.o1, valueTuple.o2);

	/// <summary>Returns a string such as "( 1, 5)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2) valueTuple, BraceType braceType) =>
		GetCachedValueFormatter(braceType).ToTupleString(valueTuple.o1, valueTuple.o2);

	/// <summary>Returns a string such as "( 1, 5)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2) valueTuple, int valueWidth, Alignment alignment) =>
		GetCachedValueFormatter(valueWidth, alignment).ToTupleString(valueTuple.o1, valueTuple.o2);

	/// <summary>Returns a string such as "( 1, 5)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2) valueTuple, int valueWidth, Alignment alignment, BraceType braceType) =>
		GetCachedValueFormatter(valueWidth, alignment, braceType).ToTupleString(valueTuple.o1, valueTuple.o2);



	/// <summary>Returns a string such as "[ 1/ 5]" with optional braces</summary>
	public static string ToValueOfCount(this (object o1, object o2) valueTuple, int valueWidth, Alignment alignment = Alignment.Right, BraceType braceType = BraceType.Square) =>
		GetCachedValueOfCountFormatter(valueWidth, alignment, braceType).ToTupleString(valueTuple.o1, valueTuple.o2);



	//// Tuple with 3 items


	/// <summary>Returns a string such as "( 1, 5, 8)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2, object o3) valueTuple) =>
		GetCachedValueFormatter().ToTupleString(valueTuple.o1, valueTuple.o2, valueTuple.o3);

	/// <summary>Returns a string such as "( 1, 5, 8)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2, object o3) valueTuple, BraceType braceType) =>
		GetCachedValueFormatter(braceType).ToTupleString(valueTuple.o1, valueTuple.o2, valueTuple.o3);

	/// <summary>Returns a string such as "( 1, 5, 8)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2, object o3) valueTuple, int valueWidth, Alignment alignment) =>
		GetCachedValueFormatter(valueWidth, alignment).ToTupleString(valueTuple.o1, valueTuple.o2, valueTuple.o3);

	/// <summary>Returns a string such as "( 1, 5, 8)" with optional braces</summary>
	public static string ToTupleString(this (object o1, object o2, object o3) valueTuple, int valueWidth, Alignment alignment, BraceType braceType) =>
		GetCachedValueFormatter(valueWidth, alignment, braceType).ToTupleString(valueTuple.o1, valueTuple.o2, valueTuple.o3);
}

