using System.Windows;

namespace OKB.Windows;

public abstract class WindowBase : Window
{
	// Define an awaitable event handler
	public delegate Task BeforeHideWindowHandler();

	/// <summary>An awaitable task that blocks the hiding of the window until completed</summary>
	public event BeforeHideWindowHandler? BeforeHideWindow;


	public bool IsShowing { get; private set; } = false;
	public bool IsHiding { get; private set; } = false;


	/// <summary>Synchronously awaited before the window is hidden to give views time to tidy up before the window is next shown</summary>
	async Task DoBeforeHideWindow()
	{
		// Check if the view has registered a before hide event handler
		if (BeforeHideWindow is null)
			return;

		// Check if the window is already hidden
		if (IsHiding)
			return;

		// Ensure we're on the same dispatcher thread as this Window
		await this.Dispatcher.Invoke(async () =>
		{
			// Because we've deferred to the dispatcher, ensure another thread didn't remove the event handler
			if (BeforeHideWindow is null)
				return;

			// Because we've deferred to the dispatcher, ensure another thread didn't hide the window already
			if (IsHiding)
				return;

			IsHiding = true;

			await BeforeHideWindow();
		});
	}

	/// <summary>Hide the window</summary>
	public virtual async void DoHideWindow()
	{
		// Check if the window is already hidden
		if (!IsShowing)
			return;

		// Check if the window is in the process of hiding already
		if (IsHiding)
			return;

		// Await any actions that need to be done before hiding the window
		await DoBeforeHideWindow();

		// Ensure we're on the same dispatcher thread as this Window
		this.Dispatcher.Invoke(() =>
		{
			// Because we've deferred to the dispatcher, ensure another thread didn't hide the window already
			if (!IsShowing)
				return;

			this.Hide();

			IsShowing = false;
			IsHiding = false;
		});
	}

	/// <summary>Show the window</summary>
	public virtual void DoShowWindow()
	{
		// Check if the window is already showing
		if (IsShowing)
			return;

		// Ensure we're on the same dispatcher thread as this Window
		this.Dispatcher.Invoke(() =>
		{
			// Because we've deferred to the dispatcher, ensure another thread didn't show the window already
			if (IsShowing)
				return;

			this.Show();
			this.Activate();

			IsShowing = true;
		});
	}
}
