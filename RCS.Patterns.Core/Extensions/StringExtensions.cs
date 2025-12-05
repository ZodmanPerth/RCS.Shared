using System.Text;

namespace System;

public static class StringExtensions
{

	//// Nullable


	extension(string? text)
	{
		/// <summary>true when the text is null or whitespace</summary>
		public bool IsNullOrWhitespace() =>
			string.IsNullOrWhiteSpace(text);
	}



	//// Not Nullable


	extension(string text)
	{
		/// <summary>Replaces slash delimiters in a folder path with a single '/' char</summary>
		public string NormaliseSlashes() => text
			.Replace(@"\", "/")
			.Replace("//", "/");

		/// <summary>returns the text as a UTF8 byte array</summary>
		public byte[] ToUTF8ByteArray()
		{
			if (text is null)
				return [];

			return Encoding.UTF8.GetBytes(text);
		}
	}
}
