namespace RCS.Helpers;

#if DEBUG

/// <summary>A static helper class used only for debugging scenarios</summary>
/// <remarks>Only available in DEBUG mode</remarks>
public static class DebugHelper
{
	/// <summary>true if the current thread is the UI thread</summary>
	public static bool IsUIThread =>
		SynchronizationContext.Current is not null;
}

#endif
