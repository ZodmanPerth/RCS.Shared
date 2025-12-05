using System;

namespace OKB.Services.AppRunTime;

/// <summary>Details about the application instance that is running and the environment it is running in</summary>
public interface IAppRunTimeService
{
	/// <summary>The version of the operating system this application is running on</summary>
	Version OSVersion { get; }

	/// <summary>The name of the application name for display to the user</summary>
	string ApplicationDisplayName { get; }

	/// <summary>The version of the running application</summary>
	Version ApplicationVersion { get; }

	/// <summary>The version of the running application as text</summary>
	string ApplicationVersionText { get; }

	/// <summary>The current runtime environment</summary>
	string Environment { get; }

	/// <summary>The folder where the application can write files</summary>
	string ApplicationRootFolderPath { get; }
}