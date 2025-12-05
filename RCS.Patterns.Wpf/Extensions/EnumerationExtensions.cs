using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace System;

public static class EnumerationExtensions
{


	//// Helpers


	static DisplayAttribute? GetDisplayAttribute(this Enum value)
	{
		var enumName = Enum.GetName(value.GetType(), value);
		if (enumName is null)
			return null;

		var fieldName = value.GetType().GetTypeInfo().GetDeclaredField(enumName);
		if (fieldName is null)
			return null;

		var attribute = fieldName.GetCustomAttribute<DisplayAttribute>();
		return attribute;
	}



	//// Actions


	/// <summary>Returns all values of the enum</summary>
	public static IEnumerable<T> GetValues<T>() =>
			Enum.GetValues(typeof(T)).Cast<T>();

	// Kudos: https://stackoverflow.com/questions/4171140/how-to-iterate-over-values-of-an-enum-having-flags
	/// <summary>Get individual flags from an input value</summary>
	/// <remarks>
	/// The enum must only contain flags (powers of 2) with an optional zero/none flag.<br></br>
	/// If your enum contains ORs of flags for convenience this extension will create unexpected results.
	/// </remarks>
	public static IEnumerable<Enum> GetFlags(this Enum input)
	{
		var zeroFlag = Enum.Parse(input.GetType(), "0");

		return Enum.GetValues(input.GetType())
			.Cast<Enum>()
			.Where
			(_ =>
				!Equals(_, zeroFlag) &&
				input.HasFlag(_)
			);
	}

	/// <summary>Return the <see cref="DisplayAttribute.Name"/> on the display attribute of the passed value</summary>
	public static string GetDisplayName(this Enum value)
	{
		var attribute = value.GetDisplayAttribute();
		if (attribute is null || attribute.Name!.IsNullOrWhitespace())
			return value.ToString();

		return attribute.Name!;
	}

	/// <summary>Return the <see cref="DisplayAttribute.ShortName"/> on the display attribute of the passed value</summary>
	public static string GetShortDisplayName(this Enum value)
	{
		var attribute = value.GetDisplayAttribute();
		if (attribute is null || attribute.ShortName!.IsNullOrWhitespace())
			return value.ToString();

		return attribute.ShortName!;
	}
}

