namespace OKB.Services;

/// <summary>An <see cref="AppContainer"/> companion for handling global app operations</summary>
public interface IAppOperations
{
	/// <summary>Shuts down the application</summary>
	void DoApplicationShutdown();

	/// <summary>Shows a new or existing window with the StatusView in it</summary>
	void ShowWindowContainingStatusView();
}
