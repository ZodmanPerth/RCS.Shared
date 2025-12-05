using OKB.Core.Types;
using System.Text;

#nullable enable

namespace OKB.Utilities;

/// <summary>Format collections values of different types into text such as "(  1,  2,  3)" or "[1, 2.2, Colors.Red]" </summary>
/// <remarks>See <see cref="TupleExtensions"/> and <see cref="ObjectExtensions"/> for extension functions that feed this formatter.</remarks>

// FEATURES:
// - Format a collection of values with a delimiter, surrounding braces, value width, and alignment within the width
// - Default formatting for numbers, dates, enums, guids, Tuples, and ValueTuples
//   - Numeric types default to
//     - 2 decimal places for types that support decimals
//     - 0 decimal places for types that do not support decimals
//   - Dates default to "yyyyMMdd"
//	 - Enum defaults to "G" (general) format
//   - Guid defaults to "D" format `00000000-0000-0000-0000-000000000000`
//   - Tuple and ValueTuple values are comma delimited and enclosed in round braces
// - Smart default delimiter to remove superfluous spaces when value width is provided
// - Configurable formatting for all explicitly supported types
// - Implicit ToString() formatting for types not explicitly supported
// - Truncated values (where value width is smaller than actual width) shows the left or right part of the value based on the alignment

// EXPLICITLY SUPPORTED TYPES
// - double
// - float
// - decimal
// - int
// - long
// - DateTime
// - DateTimeOffset
// - Enum
// - Guid
// - Tuple (up to 5 values)
// - ValueTuple (up to 5 values)

// CONFIGURABLES:
// - ToString format of all explicitly supported types
// - Brace type
// - Value width (including null for no value width)
// - Left/Right alignment within value width (defaults to right)
// - Delimiter between values (defaults to ", " or "," when value width is provided)

public class ValueFormatter
{
	string? _doubleFormat = null;
	string? _floatFormat = null;
	string? _decimalFormat = null;
	string _intFormat = "N0";
	string _longFormat = "N0";
	string _numberFormat = "N2";

	string? _dateTimeFormat = null;
	string? _dateTimeOffsetFormat = null;
	string _dateFormat = "yyyyMMdd";

	string _enumFormat = "G";
	string _guidFormat = "D";


	BraceType _braceType = BraceType.Round;
	int? _valueWidth = null;
	Alignment _alignment = Alignment.Right;
	string _delimiter = ", ";

	bool _isUsingDefaultDelimiter = true;



	//// Lifecycle


	/// <summary>Constructor with optional brace type.</summary>
	/// <remarks>Defaults brace type to round, smart comma delimiter, no value width, and alignment to right</remarks>
	public ValueFormatter(BraceType braceType = BraceType.Round)
	{
		_braceType = braceType;
	}

	/// <summary>Constructor with value width.  Optional alignment and brace type.</summary>
	/// <remarks>Defaults smart comma delimiter, brace type to round, and alignment to right</remarks>
	public ValueFormatter(int valueWidth, Alignment alignment = Alignment.Right, BraceType braceType = BraceType.Round)
	{
		_valueWidth = valueWidth;
		_alignment = alignment;
		_braceType = braceType;
	}



	//// Helpers


	public StringBuilder ToTupleStringNoBraces(params object?[] values)
	{
		// Smart modify delimter when value width is provided and we're using the default delimiter
		var isValueWidth = _valueWidth is not null;
		var isSmartModifyDelimiter = _isUsingDefaultDelimiter && !isValueWidth;
		var delimiter = isSmartModifyDelimiter
			? ","
			: _delimiter;


		var sb = new StringBuilder();
		for (var valueIndex = 0; valueIndex < values.Length; valueIndex++)
		{
			var valueText = values[valueIndex] switch
			{
				double d => ToString(d),
				float f => ToString(f),
				decimal m => ToString(m),
				int i => ToString(i),
				long l => ToString(l),
				DateTime dt => ToString(dt),
				DateTimeOffset dto => ToString(dto),
				Enum e => ToString(e),
				Guid g => ToString(g),

				ValueTuple<object> valueTuple => ToString(valueTuple),
				ValueTuple<object, object> valueTuple => ToString(valueTuple),
				ValueTuple<object, object, object> valueTuple => ToString(valueTuple),
				ValueTuple<object, object, object, object> valueTuple => ToString(valueTuple),
				ValueTuple<object, object, object, object, object> valueTuple => ToString(valueTuple),

				Tuple<object> tuple => ToString(tuple),
				Tuple<object, object> tuple => ToString(tuple),
				Tuple<object, object, object> tuple => ToString(tuple),
				Tuple<object, object, object, object> tuple => ToString(tuple),
				Tuple<object, object, object, object, object> tuple => ToString(tuple),

				var value =>
					ApplyFinalFormatting((value ?? "").ToString())

			} ?? "";

			sb.Append(valueText);
			if (valueIndex < values.Length - 1)
				sb.Append(delimiter);
		}

		return sb;
	}

	/// <summary>Provides final formatting of a value to text.  Handles padding, alignment, and truncation.</summary>
	string ApplyFinalFormatting(string? valueText)
	{
		valueText ??= "";

		if (_valueWidth is null)
			return valueText;

		var valueWidth = _valueWidth.Value;
		var valueLength = valueText.Length;

		if (valueLength > valueWidth)   // truncate
		{
			// Show left or right part of value, based on alignment
			return _alignment == Alignment.Left
				? valueText[.._valueWidth.Value]
				: valueText[(valueLength - valueWidth)..];
		}

		// Pad the value with spaces on the correct side
		return _alignment == Alignment.Left
			? valueText.PadRight(valueWidth)
			: valueText.PadLeft(valueWidth);
	}



	//// Configuration - non-type properties


	/// <summary>Specify the brace type</summary>
	public ValueFormatter WithBraceType(BraceType braceType)
	{
		_braceType = braceType;
		return this;
	}

	/// <summary>Specify the number of characters a value can use</summary>
	public ValueFormatter WithValueWidth(int width)
	{
		_valueWidth = width;
		return this;
	}

	/// <summary>Specify the alignment of the value within the value width</summary>
	public ValueFormatter WithAlignment(Alignment alignment)
	{
		_alignment = alignment;
		return this;
	}

	/// <summary>Specify the delimiter used between multiple values</summary>
	public ValueFormatter WithDelimiter(string delimiter)
	{
		_delimiter = delimiter;
		_isUsingDefaultDelimiter = false;
		return this;
	}



	//// Configuration - type formatting


	/// <summary>Specify the ToString() format for decimals</summary>
	public ValueFormatter WithDecimalFormat(string format)
	{
		_decimalFormat = format;
		return this;
	}

	/// <summary>Specify the ToString() format for floats</summary>
	public ValueFormatter WithFloatFormat(string format)
	{
		_numberFormat = format;
		return this;
	}

	/// <summary>Specify the ToString() format for double</summary>
	public ValueFormatter WithDoubleFormat(string format)
	{
		_doubleFormat = format;
		return this;
	}

	/// <summary>Specify the ToString() format for general numbers</summary>
	/// <remarks>This format will be used when the format for more specific types is not specified.</remarks>
	public ValueFormatter WithNumberFormat(string format)
	{
		_numberFormat = format;
		return this;
	}


	/// <summary>Specify the ToString() format for DateTime</summary>
	public ValueFormatter WithDateTimeFormat(string format)
	{
		_dateTimeFormat = format;
		return this;
	}

	/// <summary>Specify the ToString() format for DateTimeOffset</summary>
	public ValueFormatter WithDateTimeOffsetFormat(string format)
	{
		_dateTimeOffsetFormat = format;
		return this;
	}

	/// <summary>Specify the ToString() format for general dates</summary>
	/// <remarks>This format will be used when the format for more specific types is not specified.</remarks>
	public ValueFormatter WithDateFormat(string format)
	{
		_dateFormat = format;
		return this;
	}


	/// <summary>Specify the ToString() format for Enum</summary>
	public ValueFormatter WithEnumFormat(string format)
	{
		_enumFormat = format;
		return this;
	}

	/// <summary>Specify the ToString() format for Guid</summary>
	public ValueFormatter WithGuidFormat(string format)
	{
		_guidFormat = format;
		return this;
	}



	//// Actions


	/// <summary>Formats a double using the current configuration</summary>
	public string ToString(double value) => ApplyFinalFormatting
	(
		_doubleFormat is not null
			? value.ToString(_doubleFormat)
			: value.ToString(_numberFormat)
	);

	/// <summary>Formats a float using the current configuration</summary>
	public string ToString(float value) => ApplyFinalFormatting
	(
		_floatFormat is not null
			? value.ToString(_floatFormat)
			: value.ToString(_numberFormat)
	);

	/// <summary>Formats a decimal using the current configuration</summary>
	public string ToString(decimal value) => ApplyFinalFormatting
	(
		_decimalFormat is not null
			? value.ToString(_decimalFormat)
			: value.ToString(_numberFormat)
	);

	/// <summary>Formats an integer using the current configuration</summary>
	public string ToString(int value) => ApplyFinalFormatting
	(
		_intFormat is not null
			? value.ToString(_intFormat)
			: value.ToString(_numberFormat)
	);

	/// <summary>Formats a long using the current configuration</summary>
	public string ToString(long value) => ApplyFinalFormatting
	(
		_longFormat is not null
			? value.ToString(_longFormat)
			: value.ToString(_numberFormat)
	);


	/// <summary>Formats a DateTime using the current configuration</summary>
	public string ToString(DateTime value) => ApplyFinalFormatting
	(
		_dateTimeFormat is not null
			? value.ToString(_dateTimeFormat)
			: value.ToString(_dateFormat)
	);

	/// <summary>Formats a DateTimeOffset using the current configuration</summary>
	public string ToString(DateTimeOffset value) => ApplyFinalFormatting
	(
		_dateTimeOffsetFormat is not null
			? value.ToString(_dateTimeOffsetFormat)
			: value.ToString(_dateFormat)
	);


	/// <summary>Formats an Enum using the current configuration</summary>
	public string ToString(Enum value) => ApplyFinalFormatting
	(
		value.ToString(_enumFormat)
	);

	/// <summary>Formats a Guid using the current configuration</summary>
	public string ToString(Guid value) => ApplyFinalFormatting
	(
		value.ToString(_guidFormat)
	);


	/// <summary>Formats a ValueTuple'1 using the current configuration</summary>
	public string ToString(ValueTuple<object> valueTuple) =>
		$"({ToTupleStringNoBraces(valueTuple.Item1)})";

	/// <summary>Formats a ValueTuple'2 using the current configuration</summary>
	public string ToString(ValueTuple<object, object> valueTuple) =>
		$"({ToTupleStringNoBraces(valueTuple.Item1)}, {ToTupleStringNoBraces(valueTuple.Item2)})";

	/// <summary>Formats a ValueTuple'3 using the current configuration</summary>
	public string ToString(ValueTuple<object, object, object> valueTuple) =>
		$"({ToTupleStringNoBraces(valueTuple.Item1)}, {ToTupleStringNoBraces(valueTuple.Item2)}, {ToTupleStringNoBraces(valueTuple.Item3)})";

	/// <summary>Formats a ValueTuple'4 using the current configuration</summary>
	public string ToString(ValueTuple<object, object, object, object> valueTuple) =>
		$"({ToTupleStringNoBraces(valueTuple.Item1)}, {ToTupleStringNoBraces(valueTuple.Item2)}, {ToTupleStringNoBraces(valueTuple.Item3)}, {ToTupleStringNoBraces(valueTuple.Item4)})";

	/// <summary>Formats a ValueTuple'5 using the current configuration</summary>
	public string ToString(ValueTuple<object, object, object, object, object> valueTuple) =>
		$"({ToTupleStringNoBraces(valueTuple.Item1)}, {ToTupleStringNoBraces(valueTuple.Item2)}, {ToTupleStringNoBraces(valueTuple.Item3)}, {ToTupleStringNoBraces(valueTuple.Item4)}, {ToTupleStringNoBraces(valueTuple.Item5)})";


	/// <summary>Formats a Tuple'1 using the current configuration</summary>
	public string ToString(Tuple<object> tuple) =>
		$"({ToTupleStringNoBraces(tuple.Item1)})";

	/// <summary>Formats a Tuple'2 using the current configuration</summary>
	public string ToString(Tuple<object, object> tuple) =>
		$"({ToTupleStringNoBraces(tuple.Item1)}, {ToTupleStringNoBraces(tuple.Item2)})";

	/// <summary>Formats a Tuple'3 using the current configuration</summary>
	public string ToString(Tuple<object, object, object> tuple) =>
		$"({ToTupleStringNoBraces(tuple.Item1)}, {ToTupleStringNoBraces(tuple.Item2)}, {ToTupleStringNoBraces(tuple.Item3)})";

	/// <summary>Formats a Tuple'4 using the current configuration</summary>
	public string ToString(Tuple<object, object, object, object> tuple) =>
		$"({ToTupleStringNoBraces(tuple.Item1)}, {ToTupleStringNoBraces(tuple.Item2)}, {ToTupleStringNoBraces(tuple.Item3)}, {ToTupleStringNoBraces(tuple.Item4)})";

	/// <summary>Formats a Tuple'5 using the current configuration</summary>
	public string ToString(Tuple<object, object, object, object, object> tuple) =>
		$"({ToTupleStringNoBraces(tuple.Item1)}, {ToTupleStringNoBraces(tuple.Item2)}, {ToTupleStringNoBraces(tuple.Item3)}, {ToTupleStringNoBraces(tuple.Item4)}, {ToTupleStringNoBraces(tuple.Item5)})";



	/// <summary>Formats a collection of values using the current configuration</summary>
	public string ToTupleString(params object?[] values)
	{
		var (openBrace, closeBrace) = _braceType.GetBraces();
		return $"{openBrace}{ToTupleStringNoBraces(values)}{closeBrace}";
	}
}

