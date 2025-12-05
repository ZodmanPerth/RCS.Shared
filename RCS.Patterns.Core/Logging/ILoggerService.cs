namespace OKB.Logging;

public interface ILoggerService
{
	/// <summary>The minimum severity level used by this service</summary>
	SeverityLevel MinimumLevel { get; }

	/// <summary>Writes a message to the log</summary>
	void Write
	(
		SeverityLevel level,
		string message,
		IList<string> keys,
		IDictionary<string, string> properties,
		string filePath,
		string member,
		int lineNumber
	);

	/// <summary>Writes an exception to the log</summary>
	void Write
	(
		Exception exception,
		bool isHandled,
		string otherInfo,
		IList<string> keys,
		IDictionary<string, string> properties,
		string filePath,
		string member,
		int lineNumber
	);

	/// <summary>Additional context for the log</summary>
	void SetContext(string property, object value);
}