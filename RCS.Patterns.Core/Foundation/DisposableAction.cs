namespace OKB;

/// <summary>Invokes an action when disposed</summary>
public class DisposableAction : IDisposable
{
	/// <summary>The action invoked when disposed</summary>
	readonly Action _action;

	/// <summary>True when disposed</summary>
	bool _isDisposed;



	//// Lifecycle


	public DisposableAction(Action action)
	{
		if (action is null) throw new ArgumentNullException(nameof(action));
		_action = action;
	}



	//// Actions


	public void Dispose()
	{
		if (_isDisposed)
			return;

		_isDisposed = true;

		_action.Invoke();
	}
}