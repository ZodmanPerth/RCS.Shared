using OKB.Models;
using OKB.Services.Native.Displays;
using System.Windows;


#nullable disable

namespace OKB.Windows.PersistedState;

/// <summary>Base class for a window that remembers and restores its size, position, etc</summary>
public abstract class PersistedStateWindowBase : WindowBase
{
	readonly IAppSettingsNative _appSettings;
	readonly INativeDisplayService _nativeDisplayService;

	readonly string _windowSettingsName;

	PersistedWindowState _persistedWindowState;



	//// Lifecycle


	public PersistedStateWindowBase
	(
		// services
		IAppSettingsNative appSettings,
		INativeDisplayService nativeDisplayService,

		// data
		string windowSettingsName
	)
	{
		_appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
		_nativeDisplayService = nativeDisplayService ?? throw new ArgumentNullException(nameof(nativeDisplayService));

		_windowSettingsName = windowSettingsName ?? throw new ArgumentNullException(nameof(windowSettingsName));

		if (appSettings.PersistedWindowStates is null)
			appSettings.PersistedWindowStates = new();

		DoReadSettings();

		// Apply read persisted state to Window (if it exists)
		if (_persistedWindowState is not null)
		{
			EnsureWindowStateIsVisibleOnNearestDisplay();

			Top = _persistedWindowState.Top;
			Left = _persistedWindowState.Left;
			Width = _persistedWindowState.Width;
			Height = _persistedWindowState.Height;

			// Sometimes starting minimised can cause issues, and starting maximised can
			// move the window to the another display.
			// If it's minimized we move it to the normal state.
			WindowState = _persistedWindowState.State == WindowState.Minimized
				? WindowState.Normal
				: _persistedWindowState.State;
		}

		Loaded += (s, e) =>
		{
			// Create persisted state
			if (_persistedWindowState is null)
			{
				_persistedWindowState = new();
				_persistedWindowState.ApplyRect(RestoreBounds);
				_persistedWindowState.State = WindowState;

				DoUpdateSettings(_persistedWindowState);
			}
		};

		SizeChanged += (s, e) => DoUpdateWindowBounds();
		LocationChanged += (s, e) => DoUpdateWindowBounds();
		StateChanged += (s, e) => DoUpdateWindowState();


		return;

		void DoReadSettings()
		{
			var persistedWindowState = appSettings.PersistedWindowStates.GetValueOrDefault(_windowSettingsName);
			if (persistedWindowState is null)
				return;

			_persistedWindowState = new(persistedWindowState);
		}

		void DoUpdateWindowBounds()
		{
			if (!IsLoaded)
				return;

			var newValue = new PersistedWindowState(_persistedWindowState);
			newValue.ApplyRect(RestoreBounds);

			DoUpdateSettings(newValue);
		}

		void DoUpdateWindowState()
		{
			if (!IsLoaded)
				return;

			var newValue = new PersistedWindowState(_persistedWindowState);
			newValue.State = WindowState;

			DoUpdateSettings(newValue);
		}

		/// <summary>Updates the app settings property</summary>
		/// <remarks>
		/// The property must be updated through the property setter first so that
		/// the check to see if the value has changed works correctly.
		/// After that we can safely update our local instance, which keeps
		/// reference to the actual values in this class.
		/// </remarks>
		void DoUpdateSettings(PersistedWindowState newValue)
		{
			// Write newValue to settings
			appSettings.PersistedWindowStates[_windowSettingsName] = newValue;

			// Notify dictionary value change
			_appSettings.PersistedWindowStatesValueUpdated();

			// Update local property value
			_persistedWindowState.ApplyState(newValue);
		}

		void EnsureWindowStateIsVisibleOnNearestDisplay()
		{
			// Get display info
			var displays = _nativeDisplayService.GetDisplays();

			var displayRects = displays.Select(_ => _.WorkBounds).ToArray();
			var windowRect = _persistedWindowState.AsRect();

			var nearestDisplayRect = windowRect.ClosestContainer(displayRects);

			var fitWindowRect = windowRect.FitTo(nearestDisplayRect);

			_persistedWindowState.ApplyRect(fitWindowRect);
		}
	}
}