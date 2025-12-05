namespace System.Windows.Threading;

public static class ThreadingExtensions
{
	/// <summary>Forces a render pass on the dispatcher</summary>
	/// <remarks>
	/// If you want the Loaded event to fire on the added control, you may need to explicitly call it:<br></br>
	/// > addedControl.ApplyTemplate();<br></br>
	/// > addedControl.DoLoaded();<br></br>
	/// <br></br>
	/// See <a href="https://stackoverflow.com/a/79674808/117797">StackOverflow</a> for more detail.
	/// </remarks>
	public static Dispatcher ForceRenderPass(this Dispatcher dispatcher)
	{
		dispatcher.Invoke(() => { }, DispatcherPriority.Render);
		return dispatcher;
	}
}

