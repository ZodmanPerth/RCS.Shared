using System.Security.Cryptography;

namespace System;

public static class ByteExtensions
{

	//// Nullable


	extension(byte[]? bytes)
	{
		/// <summary>returns the text as a UTF8 byte array</summary>
		public string? ToBase64String()
		{
			if (bytes is null)
				return null;

			return Convert.ToBase64String(bytes);
		}
	}



	//// Not Nullable


	extension(byte[] bytes)
	{
		/// <summary>returns the hash of the passed bytes</summary>
		public byte[] ToHash(HashAlgorithm hashAlgorithm) =>
			hashAlgorithm.ComputeHash(bytes);
	}
}
