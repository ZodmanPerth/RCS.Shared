namespace RCS.Services.WindowFactory;

public class ViewWindowOperations
{
	/// <summary>Close the view's parent window</summary>
	public Action DoCloseWindow { get; }

	public ViewWindowOperations(Action doCloseAction)
	{
		DoCloseWindow = doCloseAction;
	}
}