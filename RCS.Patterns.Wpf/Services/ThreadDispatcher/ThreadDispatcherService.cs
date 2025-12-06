using RCS.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace RCS.Services;

/// <inheritdoc cref="IThreadDispatcherService" />
public class ThreadDispatcherService : IThreadDispatcherService
{
	readonly Dispatcher _dispatcher;

	public event Action<Exception>? UnhandledException;


	public bool IsOnUIThread => Application.Current.Dispatcher == _dispatcher;



	//// Lifecycle


	/// <remarks>Must be created on the ui thread</remarks>
	public ThreadDispatcherService()
	{
		_dispatcher = Application.Current.Dispatcher;
		if (_dispatcher is null)
			throw new InvalidOperationException("Can't get UI dispatcher");

		_dispatcher.UnhandledException += OnDispatcherUnhandledException;
	}



	//// Event Handlers


	void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		Logger.AsError("(ThreadDispatcher) Unhandled dispatcher exception")
			.WithException(e.Exception)
			.Write();

		e.Handled = true;
	}



	//// Actions


	[DebuggerHidden]
	public Task QueueActionOnUIThread
	(
		Action action,
		DispatcherPriority priority = DispatcherPriority.Normal,
		[CallerMemberName] string? caller = null,
		[CallerLineNumber] int? callerLineNumber = null
	)
	{
		return _dispatcher.BeginInvoke(() =>
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				ex.Data["Caller"] = caller;
				ex.Data["CallerLineNumber"] = callerLineNumber;

				Logger.AsError("Unhandled exception during fire and forget")
					.WithException(ex)
					.WithProperty("Caller", caller)
					.WithProperty("CallerLineNumber", callerLineNumber)
					.Write();

				UnhandledException?.Invoke(ex);
			}
		}, priority).Task;
	}

	[DebuggerHidden]
	async public void RunOnUIThread
	(
		Action action,
		DispatcherPriority priority = DispatcherPriority.Normal,
		bool isUseCurrentThread = false,
		[CallerMemberName] string? caller = null,
		[CallerLineNumber] int? callerLineNumber = null
	)
	{
		if (action is null)
			return;

		try
		{
			if (isUseCurrentThread && IsOnUIThread)
			{
				action.Invoke();
			}
			else
			{
				var taskCompletionSource = new TaskCompletionSource<object?>();

				_dispatcher.Invoke(() =>
				{
					try
					{
						action.Invoke();
						taskCompletionSource.SetResult(null);
					}
					catch (Exception e)
					{
						taskCompletionSource.SetException(e);
					}
				}, priority);

				await taskCompletionSource.Task.ConfigureAwait(false);
			}
		}
		catch (Exception ex)
		{
			ex.Data["Caller"] = caller;
			ex.Data["CallerLineNumber"] = callerLineNumber;

			Logger.AsError("Unhandled exception during fire and forget")
				.WithException(ex)
				.WithProperty("Caller", caller)
				.WithProperty("CallerLineNumber", callerLineNumber)
				.Write();

			UnhandledException?.Invoke(ex);
		}
	}

	[DebuggerHidden]
	public void QueueActionOnThreadPool(Action action) =>
		ThreadPool.QueueUserWorkItem((_) => action());

	[DebuggerHidden]
	public void QueueActionOnThreadPool(Action<object?> action) =>
		ThreadPool.QueueUserWorkItem((_) => action(_));
}

