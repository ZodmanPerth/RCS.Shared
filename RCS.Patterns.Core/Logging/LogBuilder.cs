using System.Runtime.CompilerServices;

#nullable disable

namespace OKB.Logging;

public class LogBuilder
{

	SeverityLevel _level = SeverityLevel.Verbose;
	string _description = string.Empty;
	Exception _exception = null;
	bool _isHandled = true;
	Dictionary<string, string> _properties = new();
	List<string> _keys = new();



	//// Fluent


	public LogBuilder WithKey(string keyValue)
	{
		_keys.Add(keyValue ?? "<null>");
		return this;
	}

	public LogBuilder WithProperty(string propertyName, object propertyValue)
	{
		_properties.Add(propertyName, propertyValue?.ToString() ?? "<null>");
		return this;
	}

	public LogBuilder WithException(Exception ex, bool isHandled = true)
	{
		_exception = ex;
		_isHandled = isHandled;
		return this;
	}

	public LogBuilder AsCritical(string description)
	{
		_description = description;
		_level = SeverityLevel.Critical;
		return this;
	}

	public LogBuilder AsError(string description)
	{
		_description = description;
		_level = SeverityLevel.Error;
		return this;
	}

	public LogBuilder AsWarning(string description)
	{
		_description = description;
		_level = SeverityLevel.Warning;
		return this;
	}

	public LogBuilder AsInformation(string description)
	{
		_description = description;
		_level = SeverityLevel.Information;
		return this;
	}

	public LogBuilder AsVerbose(string description)
	{
		_description = description;
		_level = SeverityLevel.Verbose;
		return this;
	}

	public LogBuilder AsTrace(string description)
	{
		_description = description;
		_level = SeverityLevel.Trace;
		return this;
	}



	//// Actions


	/// <summary>Writes to the log</summary>
	public void Write
	(
		[CallerFilePath] string filePath = null,
		[CallerMemberName] string member = null,
		[CallerLineNumber] int lineNumber = 0
	)
	{
		if (_exception is not null)
		{
			if (_level >= SeverityLevel.Error)
			{
				Logger.Write(_exception, _isHandled, _description, filePath, member, lineNumber, _keys, _properties);
				return;
			}

			_properties["message"] = _exception.Message;
		}

		Logger.Write(_level, _description, filePath, member, lineNumber, _keys, _properties);
	}
}
