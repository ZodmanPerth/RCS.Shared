namespace System;

public static class IntExtensions
{
	/// <summary>True if the value is between the low and high values (inclusive)</summary>  
	public static bool Between(this int value, int low, int high) =>
		value >= low && value <= high;

	/// <summary>The value clempted to a specified mimimum and maximum</summary>  
	public static int Clamp(this int value, int minimum, int maximum) =>
		value switch
		{
			_ when value < minimum => minimum,
			_ when value > maximum => maximum,
			_ => value
		};

	/// <summary>The value clempted to a specified maximum</summary>  
	public static int ClampMaximum(this int value, int maximum) =>
		value switch
		{
			_ when value > maximum => maximum,
			_ => value
		};

	/// <summary>The value clampted to a specified mimimum</summary>  
	public static int ClampMinimum(this int value, int minimum) =>
		value switch
		{
			_ when value < minimum => minimum,
			_ => value
		};
}
