using OKB.Logging;
using System.Diagnostics;

namespace OKB.Patterns;

/// <summary>Directs trace messages to the logger</summary>
// KUDOS: https://stackoverflow.com/a/19610447/117797
public class TraceListenerLogger : TraceListener
{
	public override void Write(string? message)
	{
		if (message is null || message.IsNullOrWhitespace())
			return;

		Logger.AsVerbose(message)
			.Write();
	}

	public override void WriteLine(string? message) =>
		Write(message);
}
