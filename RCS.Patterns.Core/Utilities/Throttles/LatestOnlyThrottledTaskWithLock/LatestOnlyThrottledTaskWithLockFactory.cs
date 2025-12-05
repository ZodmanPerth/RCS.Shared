using System.Reactive.Concurrency;
using AsyncLock = Nito.AsyncEx.AsyncLock;

namespace OKB.Utilities;

/// <summary>A factory to simplify the creation of <see cref="LatestOnlyThrottledTaskWithLock<>"/></summary>
public class LatestOnlyThrottledTaskWithLockFactory<T>
{
	public LatestOnlyThrottledTaskWithLock<T> Create(Func<T?, Task> task, TimeSpan timeSpan, AsyncLock asyncLock) =>
		new LatestOnlyThrottledTaskWithLock<T>(NewThreadScheduler.Default, task, timeSpan, asyncLock);
}