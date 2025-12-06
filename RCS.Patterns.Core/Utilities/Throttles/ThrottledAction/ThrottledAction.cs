using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RCS.Utilities;

/// <summary>
/// Fires an action after waiting for a delay to pass without further calls to fire the action.<br></br>
/// Supports pause and continue.
/// </summary>
public class ThrottledAction : IDisposable
{
	readonly Action _action;

	Subject<bool> _requestThrottledAction;
	IDisposable _subscriptionForActionRequest;      // keep subscription in memory

	bool _isPaused = false;
	bool _isEventObservedDuringPause = false;



	//// Lifecycle


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	ThrottledAction(Action action)
	{
		_action = action ?? throw new ArgumentNullException(nameof(action));
		_requestThrottledAction = new();
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	public ThrottledAction(IScheduler scheduler, Action action, TimeSpan delay)
		: this(action)
	{
		if (scheduler is null) throw new ArgumentNullException(nameof(scheduler));

		_subscriptionForActionRequest = _requestThrottledAction
			.Throttle(delay)
			.ObserveOn(scheduler)
			.Subscribe(_ => DoAction());
	}

	public ThrottledAction(SynchronizationContext synchronisationContext, Action action, TimeSpan delay)
		: this(action)
	{
		if (synchronisationContext is null) throw new ArgumentNullException(nameof(synchronisationContext));

		_subscriptionForActionRequest = _requestThrottledAction
			.Throttle(delay)
			.ObserveOn(synchronisationContext)
			.Subscribe(_ => DoAction());

		return;


		//// Local Functions


	}

	public void Dispose()
	{
		_subscriptionForActionRequest?.Dispose();
		_requestThrottledAction?.Dispose();
	}



	//// Helpers


	void DoAction()
	{
		if (_isPaused)
		{
			_isEventObservedDuringPause = true;
			return;
		}

		_action.Invoke();
	}



	//// Actions


	/// <summary>Requests the throttled action to be invoked after waiting for the throttled delay to pass uninterrupted</summary>
	public void Invoke() =>
		_requestThrottledAction.OnNext(true);

	public void Pause() =>
		_isPaused = true;

	public void Continue()
	{
		if (!_isPaused)
			return;

		_isPaused = false;

		if (_isEventObservedDuringPause)
		{
			// Clear flag
			_isEventObservedDuringPause = false;

			// Events were observed while paused.
			// Fire immediately the action immediately.
			_action.Invoke();
		}
	}

}