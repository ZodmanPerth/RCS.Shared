using RCS.Services.Dtos;
using RCS.Services.Obs;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text.Json;

namespace RCS.WebSockets;

public class ObsWebSocket
{
	const string ObsWebServerUriText = @"ws://localhost:4455";
	const string ObsWebSocketPassword = @"YO89T51e3uKGe4MZ";

	readonly JsonSerializerOptions jsonSerializerOptionsIncoming = new()
	{
		PropertyNameCaseInsensitive = true,
		WriteIndented = true
	};

	readonly JsonSerializerOptions jsonSerializerOptionsOutgoing = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true
	};


	WebSocketWrapper? _webSocket;



	//// Helpers


	async Task InitiateOpenConnectionProtocol()
	{
		if (!await ConnectWebSocket())
			return;

		// Carl TODO: start message loop

		return;


		//// Local Functions


		/// <summary>true when successfully connected</summary>
		async Task<bool> ConnectWebSocket()
		{
			var isAlreadyConnected = _webSocket is not null;
			if (isAlreadyConnected)
				return true;

			try
			{
				_webSocket = new();

				await _webSocket.ConnectAsync(new Uri(ObsWebServerUriText), CancellationToken.None);
				if (!await DoAuthenticationHandshake())
				{
					await CloseWebSocket();
					return false;
				}

				return true;
			}
			catch
			{
				Debug.WriteLine($"Can't connect");
				await CloseWebSocket();
				return false;
			}
		}

		/// <summary>true if handshake was successful</summary>
		async Task<bool> DoAuthenticationHandshake()
		{
			if (_webSocket.State != WebSocketState.Open)
				return false;	

			var helloMessage = await _webSocket.ReceiveMessage();
			if (helloMessage is null)
				return false;

			if (!await SendIdentifyMessage(helloMessage))
				return false;

			var identifiedMessage = await _webSocket.ReceiveMessage();
			if (identifiedMessage is null)
				return false;

			var identified = JsonSerializer.Deserialize<OpCode2>(identifiedMessage, jsonSerializerOptionsIncoming);
			if (identified is null)
				return false;

			Debug.WriteLine($"Negotiated Rpc Version: {identified.D.NegotiatedRpcVersion}");

			return true;

		}

		async Task<bool> SendIdentifyMessage(string helloMessage)
		{
			if (_webSocket is null)
				return false;

			var hello = JsonSerializer.Deserialize<OpCode0>(helloMessage, jsonSerializerOptionsIncoming);
			if (hello is null)
				return false;

			// Construct authentication text
			var authentication = hello.D.Authentication;
			var authenticationText = ObsWebSocketPassword;
			if (authentication is not null)
			{
				var saltedPassword = ObsWebSocketPassword + authentication.Salt;
				using (var sha256 = SHA256.Create())
				{
					var saltedPaswordHash = saltedPassword.ToUTF8ByteArray().ToHash(sha256);
					var base64Secret = saltedPaswordHash.ToBase64String();
					var challengeConcatenation = base64Secret + authentication.Challenge;
					var challengeContatenationHash = challengeConcatenation.ToUTF8ByteArray().ToHash(sha256);
					authenticationText = challengeContatenationHash.ToBase64String();
				}
			}

			// Construct identification message
			var identify = new OpCode1
			(
				Op: 1,
				D: new OpCode1_D
				(
					RpcVersion: 1,
					Authentication: authenticationText,
					EventSubscriptions: (int)EventSubscription.AllLowFreqency
				)
			);

			// Send message
			string json = JsonSerializer.Serialize(identify, jsonSerializerOptionsOutgoing);
			return await _webSocket.SendMessage(json);
		}
	}



	async Task CloseWebSocket()
	{
		if (_webSocket is null)
			return;

		if (_webSocket.State != WebSocketState.Open)
			return;

		await _webSocket.DisposeAsync();
		_webSocket = null;
	}



	//// Actions


	public async Task Connect() =>
		await InitiateOpenConnectionProtocol();

}
