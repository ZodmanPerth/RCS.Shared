namespace RCS.Services.UriNavigator;

/// <summary>Handles Uri Activation</summary>
public interface IUriNavigatorService
{
	/// <summary>True if the uriText is opened in the system's default handler</summary>
	bool NavigateTo(string uriText, bool isExternal = true);
}