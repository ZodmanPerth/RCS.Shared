using System.Diagnostics;
using System.Globalization;

namespace RCS.Services.UriNavigator;

public class UriNavigatorService : IUriNavigatorService
{
	public bool NavigateTo(string uriText, bool isExternal = true)
	{
		if (uriText.IsNullOrWhitespace())
			return false;

		if (uriText.StartsWith("www", ignoreCase: true, CultureInfo.InvariantCulture))
			uriText = $"http://{uriText}";

		var processStartInfo = new ProcessStartInfo(uriText) { UseShellExecute = isExternal };

		try
		{
			Process.Start(processStartInfo);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
