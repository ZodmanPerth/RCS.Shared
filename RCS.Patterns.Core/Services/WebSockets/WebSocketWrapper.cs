using System.Diagnostics;
using System.Net.WebSockets;

namespace RCS.WebSockets;

/// <summary>A websocket wrapped with exception handling</summary>
public class WebSocketWrapper : IAsyncDisposable
{
	internal ClientWebSocket _webSocket = new();


	public WebSocketState State => _webSocket?.State ?? WebSocketState.None;



	//// Lifecycle


	public async ValueTask DisposeAsync()
	{
		if (_webSocket is null)
			return;

		if (_webSocket.State != WebSocketState.Open)
			return;

		await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, statusDescription: "Closing", CancellationToken.None);

		_webSocket.Dispose();
	}



	//// Actions


	/// <summary>Connects to the given URI</summary>
	public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
	{
		try
		{
			await _webSocket.ConnectAsync(uri, cancellationToken);
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"WebSocket connect error: {ex.Message}");
		}
	}


	/// <summary>true if the message was sent</summary>
	public async Task<bool> SendMessage(string text, CancellationToken cancellationToken = default)
	{
		if (_webSocket is null)
			return false;

		if (text.IsNullOrWhitespace())
			return false;

		var buffer = text.ToUTF8ByteArray();
		try
		{
			await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, endOfMessage: true, cancellationToken);
			return true;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"WebSocket send error: {ex.Message}");
			return false;
		}
	}

	/// <summary>Returns the received message or null</summary>
	public async Task<string?> ReceiveMessage(CancellationToken cancellationToken = default)
	{
		if (_webSocket is null)
			return null;

		try
		{
			var buffer = new byte[4096];
			var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

			if (result.MessageType != WebSocketMessageType.Text)
				return null;

			var message = result.ToUTF8String(buffer);
			return message;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"WebSocket receive error: {ex.Message}");
			return null;
		}
	}
}
