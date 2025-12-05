using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using AsyncLock = Nito.AsyncEx.AsyncLock;

namespace OKB.Utilities;

/// <summary>
/// Fires a task after waiting for a delay to pass without further calls to fire the task.<br></br>
/// Takes a lock before processes the task with the latest value.<br></br>
/// Will check again after completion of the task to see if further values arrived while processing.
/// </summary>
public class LatestOnlyThrottledTaskWithLock<T> : IDisposable
{
	readonly Func<T?, Task> _task;

	Subject<T?> _requestThrottledTask;
	IDisposable _subscriptionForTaskRequest;      // keep subscription in memory

	T? _latestValue = default(T);
	bool _isProcessing = false;
	AsyncLock _mutex;



	//// Lifecycle


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	LatestOnlyThrottledTaskWithLock(Func<T?, Task> taskToInvokeUnderLock, AsyncLock mutex)
	{
		_task = taskToInvokeUnderLock ?? throw new ArgumentNullException(nameof(taskToInvokeUnderLock));
		_mutex = mutex ?? throw new ArgumentNullException(nameof(mutex));
		_requestThrottledTask = new();
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	public LatestOnlyThrottledTaskWithLock(IScheduler scheduler, Func<T?, Task> taskToInvokeUnderLock, TimeSpan delay, AsyncLock asyncLock)
		: this(taskToInvokeUnderLock, asyncLock)
	{
		if (scheduler is null) throw new ArgumentNullException(nameof(scheduler));

		_subscriptionForTaskRequest = _requestThrottledTask
			.Throttle(delay)
			.ObserveOn(scheduler)
			.Subscribe(value =>
			{
				_latestValue = value;
				_ = TryProcessLatest();
			});
	}

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
	public void Dispose()
	{
		_subscriptionForTaskRequest?.Dispose();
		_requestThrottledTask?.Dispose();
	}
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize



	//// Helpers


	/// <summary>Processes the latest value (if there is one) under the lock.</summary>
	async Task TryProcessLatest()
	{
		// If we're already processing or there is no value, return
		if (_isProcessing || _latestValue is null) return;

		// Process the value
		_isProcessing = true;
		using (await _mutex.LockAsync())
		{
			// Clear latest value
			var valueToProcess = _latestValue;
			_latestValue = default(T);

			// Await the throttled task
			await _task(valueToProcess);
		}
		_isProcessing = false;

		// If a new value arrived while we were executing, try to process it.
		// We don't want to await here.  If the action was slow enough for a throttle to complete
		// we don't want to prevent the concurrent task from completing.
		var isDefault = EqualityComparer<T?>.Default.Equals(_latestValue, default);
		if (!isDefault)
			_ = TryProcessLatest();
	}



	//// Actions


	/// <summary>Requests the throttled task to be invoked after waiting for the throttled delay to pass uninterrupted</summary>
	public void Request(T? value) =>
		_requestThrottledTask.OnNext(value);
}

