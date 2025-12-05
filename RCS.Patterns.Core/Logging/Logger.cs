using System.Runtime.CompilerServices;

#nullable disable

namespace OKB.Logging;

public static class Logger
{

	static List<ILoggerService> _services = new();



	//// Actions - attach/detatch services


	/// <summary>Attaches a logging service</summary>
	public static void Attach(ILoggerService service)
	{
		if (service is null)
			return;

		if (_services.Contains(service))
			return;

		_services.Add(service);
	}

	/// <summary>Detaches a logging service</summary>
	public static void Detach(ILoggerService service)
	{
		if (service is null)
			return;

		_services.Remove(service);
	}



	//// Actions


	/// <summary>Writes the message to the log</summary>
	public static void Write
	(
		SeverityLevel level,
		string message,
		[CallerFilePath] string filePath = null,
		[CallerMemberName] string member = null,
		[CallerLineNumber] int lineNumber = 0,
		IList<string> keys = null,
		IDictionary<string, string> properties = null
	)
	{
		if (_services is null)
			return;

		foreach (var service in _services)
			service.Write(level, message, keys, properties, filePath, member, lineNumber);
	}

	/// <summary>Enters a single entry in all attached services, if the <paramref name="level"/> is supported.</summary>
	/// <param name="level">The severity level of the message.</param>
	/// <param name="message">A function that returns the contents of the message.</param>
	/// <param name="filePath">The file location of the calling member.</param>
	/// <param name="member">The name of the calling member.</param>
	/// <param name="lineNumber">The line number where the error was notified.</param>
	/// <param name="keys">Optional key value pair for tracking app systems in the logs.</param>
	/// <param name="properties">Optional key value pair properties.</param>
	/// <remarks>Use this method to be more efficient when logging <see cref="SeverityLevel.Verbose"/> content.</remarks>
	public static void Write
	(
		SeverityLevel level,
		Func<string> message,
		[CallerFilePath] string filePath = null,
		[CallerMemberName] string member = null,
		[CallerLineNumber] int lineNumber = 0,
		IList<string> keys = null,
		IDictionary<string, string> properties = null
	)
	{
		if (_services is null)
			return;

		string messageContent = null;

		foreach (var service in _services)
		{
			if (service.MinimumLevel > level)
				continue;

			if (messageContent is null)
				messageContent = message();

			service.Write(level, messageContent, keys, properties, filePath, member, lineNumber);
		}
	}

	/// <summary>Records an exception within the application in all attached services.</summary>
	/// <param name="exception">The handled or unhandled exception.</param>
	/// <param name="isHandled">Determines if the <paramref name="exception"/> was handled by the application.</param>
	/// <param name="otherInfo">An optional description of the current environment to assist with debugging.</param>
	/// <param name="filePath">The file location of the calling member.</param>
	/// <param name="member">The name of the calling member.</param>
	/// <param name="lineNumber">The line number where the error was notified.</param>
	/// <param name="properties">Optional key value pair properties.</param>
	public static void Write
	(
		Exception exception,
		bool isHandled,
		string otherInfo = null,
		[CallerFilePath] string filePath = null,
		[CallerMemberName] string member = null,
		[CallerLineNumber] int lineNumber = 0,
		IList<string> keys = null,
		IDictionary<string, string> properties = null
	)
	{
		if (_services is null)
			return;

		foreach (var service in _services)
			service.Write(exception, isHandled, otherInfo, keys, properties, filePath, member, lineNumber);
	}

	/// <summary>
	/// Creates a LogBuilder that adds the specified property to the log event.
	/// </summary>
	/// <param name="propertyName">The property name</param>
	/// <param name="propertyValue">The property value (will be converted to string)</param>
	/// <returns>A new LogBuilder</returns>
	public static LogBuilder WithProperty(string propertyName, object propertyValue) => new LogBuilder().WithProperty(propertyName, propertyValue);

	/// <summary> Creates a LogBuilder that adds the specified exception to the log event. </summary>
	public static LogBuilder WithException(Exception e, bool isHandled = true) => new LogBuilder().WithException(e, isHandled);

	/// <summary>
	/// Creates a LogBuilder that adds the specified description to the log event with a critical severity level.
	/// </summary>
	/// <param name="description">The log description</param>
	/// <returns>A new LogBuilder</returns>
	public static LogBuilder AsCritical(string description) => new LogBuilder().AsCritical(description);

	/// <summary>
	/// Creates a LogBuilder that adds the specified description to the log event with an error severity level.
	/// </summary>
	/// <param name="description">The log description</param>
	/// <returns>A new LogBuilder</returns>
	public static LogBuilder AsError(string description) => new LogBuilder().AsError(description);

	/// <summary>
	/// Creates a LogBuilder that adds the specified description to the log event with a warning severity level.
	/// </summary>
	/// <param name="description">The log description</param>
	/// <returns>A new LogBuilder</returns>
	public static LogBuilder AsWarning(string description) => new LogBuilder().AsWarning(description);

	/// <summary>
	/// Creates a LogBuilder that adds the specified description to the log event with an information severity level.
	/// </summary>
	/// <param name="description">The log description</param>
	/// <returns>A new LogBuilder</returns>
	public static LogBuilder AsInformation(string description) => new LogBuilder().AsInformation(description);

	/// <summary>
	/// Creates a LogBuilder that adds the specified description to the log event with a verbose severity level.
	/// </summary>
	/// <param name="description">The log description</param>
	/// <returns>A new LogBuilder</returns>
	public static LogBuilder AsVerbose(string description) => new LogBuilder().AsVerbose(description);

	/// <summary>
	/// Creates a LogBuilder that adds the specified description to the log event with a debug severity level.
	/// </summary>
	/// <param name="description">The log description</param>
	/// <returns>A new LogBuilder</returns>
	public static LogBuilder AsTrace(string description) => new LogBuilder().AsTrace(description);
}
