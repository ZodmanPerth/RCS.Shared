namespace RCS.Utilities;

/// <summary>A factory to simplify the creation of <see cref="ThrottledAction"/></summary>
public class ThrottledActionFactory
{
	readonly Func<ThrottledActionThread, Action, TimeSpan, ThrottledAction> _throttledActionFactory;



	//// Lifecycle


	public ThrottledActionFactory(Func<ThrottledActionThread, Action, TimeSpan, ThrottledAction> throttledActionFactory) =>
		_throttledActionFactory = throttledActionFactory ?? throw new ArgumentNullException(nameof(throttledActionFactory));


	//// Actions


	public ThrottledAction CreateToRunOnBackgroundThreads(Action action, TimeSpan timeSpan) =>
		_throttledActionFactory.Invoke(ThrottledActionThread.BackgroundThread, action, timeSpan);

	public ThrottledAction CreateToRunOnUIThread(Action action, TimeSpan timeSpan) =>
		_throttledActionFactory.Invoke(ThrottledActionThread.UIThread, action, timeSpan);
}