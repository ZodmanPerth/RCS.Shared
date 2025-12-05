using System.Windows.Media.Animation;

namespace OKB.Utilities;

/// <summary>
/// Monitors the clock from an animation for key frames.<br></br>
/// Fires callback for each key frame is passed.<br></br>
/// Stops monitoring automatically when the last frame is processed.
/// </summary>

// To use:															Example code:
// * Create a clock from an animation								var clock = translateXAnimation.CreateClock();
// * Apply the clock to the property the animation is animating		myElement.ApplyAnimationClock(TranslateTransform.XProperty, clock);
// * Create the key frame monitor									var animationKeyFrameMonitor = new AnimationKeyFrameMonitor(clock, keyFrameTimes, OnKeyFrameChanged);

// Call `Stop()` to stop monitoring.

public class AnimationKeyFrameMonitor
{
	readonly Clock _clock;
	readonly List<TimeSpan> _keyFrameTimes;
	readonly Action<int, int> _callbackOnKeyFrameReached;   // frame, total frames

	int _firedIndex = 0;



	//// Lifecycle


	/// <summary>Create a new monitor.  See notes on class for instructions.</summary>
	/// <remarks><paramref name="keyFrameTimes"/> should include the time at the end of the animation for counting purposes</remarks>
	public AnimationKeyFrameMonitor
	(
		Clock clock,
		IEnumerable<TimeSpan> keyFrameTimes,
		Action<int, int> callbackOnKeyFrameReached
	)
	{
		_clock = clock ?? throw new ArgumentNullException(nameof(clock));
		if (keyFrameTimes is null) throw new ArgumentNullException(nameof(keyFrameTimes));
		_callbackOnKeyFrameReached = callbackOnKeyFrameReached ?? throw new ArgumentNullException(nameof(callbackOnKeyFrameReached));

		_keyFrameTimes = keyFrameTimes.ToList();
		_keyFrameTimes.Sort();

		_clock.CurrentTimeInvalidated += ClockTick;
	}

	/// <remarks>The clock tick event will fire as fast as possible.  It's up to us to find the keyframes.</remarks>
	void ClockTick(object? sender, EventArgs e)
	{
		var currentTime = _clock.CurrentTime;
		if (currentTime == null) return;

		// Time only moves forward and keyframetimes are stored in increasing order.
		// We keep track of the index we last passed to prevent needing to iterate through all keyframes every tick.
		for (int i = _firedIndex; i < _keyFrameTimes.Count; i++)
		{
			var keyTime = _keyFrameTimes[i];
			if (currentTime < keyTime)
				break;  // abort

			_firedIndex++;
			_callbackOnKeyFrameReached.Invoke(i, _keyFrameTimes.Count - 1);     // The last keyframe time is the end of the animation
		}

		// Check if we can stop monitoring the clock
		if (_firedIndex == _keyFrameTimes.Count)
			Stop();
	}

	/// <summary>Stop monitoring the clock</summary>
	public void Stop() =>
		_clock.CurrentTimeInvalidated -= ClockTick;
}
