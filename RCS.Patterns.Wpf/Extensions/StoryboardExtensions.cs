namespace System.Windows.Media.Animation;

#nullable enable

public static class StoryboardExtensions
{
	/// <summary>
	/// Stops a storyboard (on the passed object if there is one) and returns when it has finished.<br></br>
	/// A timeout can be specified (defaults to one second) to return if the storyboard does not stop within the specified time.
	/// </summary>
	public static Task StopStoryboardAsync(this Storyboard storyboard, FrameworkElement? containingObject, TimeSpan? timeout = null)
	{
		if (storyboard is null)
			throw new ArgumentNullException(nameof(storyboard));

		// Create a controllable Task
		var taskCompletionSource = new TaskCompletionSource<bool>();

		// Configure timeout
		timeout ??= TimeSpan.FromSeconds(1);
		var timeoutTimer = new Timers.Timer(timeout.Value.TotalMilliseconds);
		timeoutTimer.AutoReset = false;
		timeoutTimer.Elapsed += (s, e) =>
			taskCompletionSource.SetResult(false);

		// Configure event that fires when the story stops
		storyboard!.CurrentStateInvalidated += OnCurrentStateInvalidated;

		// Stop the storyboard
		if (containingObject is null)
			storyboard.Stop();
		else
			storyboard!.Stop(containingObject);

		// Start the timeout timer
		timeoutTimer.Start();

		// Return the task to be awaited
		return taskCompletionSource.Task;


		//// Local Functions


		void OnCurrentStateInvalidated(object? sender, EventArgs e)
		{
			timeoutTimer.Stop();

			storyboard!.CurrentStateInvalidated -= OnCurrentStateInvalidated;

			taskCompletionSource.SetResult(true);
		};
	}
}

