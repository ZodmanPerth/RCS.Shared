using System.IO;
using System.Reflection;
using Windows.System.Profile;


#nullable disable

namespace OKB.Services.AppRunTime;

/// <inheritdoc cref="IAppRunTimeService" />
public class AppRunTimeService : IAppRunTimeService
{
	public const string EnvironmentNameDevelopment = "DEV";
	public const string EnvironmentNameProduction = "PROD";

	public Version OSVersion { get; private set; }
	public string ApplicationDisplayName { get; private set; }
	public Version ApplicationVersion { get; private set; }
	public string ApplicationVersionText { get; private set; }


	public string Environment { get; private set; }

	public string ApplicationRootFolderPath { get; private set; }



	//// Lifecycle


	public AppRunTimeService(string applicationDisplayName)
	{
		ApplicationDisplayName = applicationDisplayName ?? throw new ArgumentNullException(nameof(applicationDisplayName));

		SetOSVersion();
		SetAssemblyVersion();
		SetEnvironment();

		ApplicationRootFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppData\\");

		return;


		//// Local Functions

		void SetAssemblyVersion()
		{
			var assemblyName = Assembly.GetExecutingAssembly().GetName();
			ApplicationVersion = assemblyName.Version;
			ApplicationVersionText = $"v{ApplicationVersion}";
		}

		void SetEnvironment()
		{
#if DEBUG
			Environment = EnvironmentNameDevelopment;
#else
			Environment = EnvironmentNameProduction;
#endif
		}

		// Kudos: https://stackoverflow.com/a/31895625/117797
		void SetOSVersion()
		{
			var deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
			var longVersion = ulong.Parse(deviceFamilyVersion);

			var major = (int)((longVersion & 0xFFFF000000000000L) >> 48);
			var minor = (int)((longVersion & 0x0000FFFF00000000L) >> 32);
			var build = (int)((longVersion & 0x00000000FFFF0000L) >> 16);
			var revision = (int)((longVersion & 0x000000000000FFFFL));

			OSVersion = new Version(major, minor, build, revision);
		}
	}
}
