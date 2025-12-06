using OKB.WebSockets;

namespace OKB.Services;

public class ObsWebService
{


	ObsWebSocket _obsWebSocket;



	//// Lifecycle


	public ObsWebService()
	{
		_obsWebSocket = new();
		_ = _obsWebSocket.Connect();
	}



	//// Actions


	public async Task Connect() =>
		await _obsWebSocket.Connect();

}
