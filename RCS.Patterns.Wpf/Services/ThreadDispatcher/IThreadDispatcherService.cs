using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace RCS.Services;

/// <summary></summary>
public interface IThreadDispatcherService
{
	/// <summary>Fired when an unhandled exception occurs</summary>
	event Action<Exception> UnhandledException;

	/// <summary>true when the current thread is the UI thread</summary>
	bool IsOnUIThread { get; }

	/// <summary>Runs an action synchronously on the UI thread</summary>
	/// <param name="isUseCurrentThread">when true and the current thread is the UI thread, runs immediately.  Otherwise queues on the UI thread.</param>
	void RunOnUIThread(Action action, DispatcherPriority priority = DispatcherPriority.Normal, bool isUseCurrentThread = false, [CallerMemberName] string? caller = null, [CallerLineNumber] int? callerLineNumber = null);

	/// <summary>Queues the action on the UI thread as a task and returns that task</summary>
	Task QueueActionOnUIThread(Action action, DispatcherPriority priority = DispatcherPriority.Normal, [CallerMemberName] string? caller = null, [CallerLineNumber] int? callerLineNumber = null);

	/// <summary>Adds the passed action to the ThreadPool for invoking</summary>
	void QueueActionOnThreadPool(Action action);

	/// <summary>Adds the passed action to the ThreadPool for invoking</summary>
	void QueueActionOnThreadPool(Action<object?> action);
}