using System.Net.WebSockets;
using System.Text;

namespace System;

public static class WebSocketExtensions
{

	//// Nullable


	extension(WebSocketReceiveResult? result)
	{
		public string? ToUTF8String(byte[]? buffer)
		{
			if (result is null || buffer is null)
				return null;

			return Encoding.UTF8.GetString(buffer, 0, result.Count);
		}
	}



	//// Not Nullable


	extension(WebSocketReceiveResult result)
	{

	}
}
