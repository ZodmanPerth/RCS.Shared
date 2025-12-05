using OKB.Utilities;
using Windows.System;

namespace OKB.Services.Native.Keyboard;

/// <summary>A service for binding actions to hotkeys.</summary>
// KUDOS: https://github.com/kfirprods/NonInvasiveKeyboardHook
public interface IHotKeyBindingService : IDisposable
{
	/// <summary>true to log key up/down messages</summary>
	bool IsLogKeyUpDown { get; }

	/// <summary>True when listening for keys</summary>
	bool IsListening { get; }


	/// <summary>Registers an action to invoke when a hotkey is activated.</summary>
	/// <returns>A disposable that unregisters the hotkey binding.</returns>
	DisposableAction AddBinding(KeyModifiers modifiers, VirtualKey virtualKey, Action hotKeyAction);

	/// <summary>Unregisters all hotkey action bindings</summary>
	void RemoveAllBindings();

	/// <summary>Unregister a specific hot key binding</summary>
	void RemoveBinding(KeyModifiers modifiers, VirtualKey virtualKey);


	/// <summary>Start listening for keys</summary>
	void StartListening();

	/// <summary>Stop listening for keys</summary>
	void StopListening();
}