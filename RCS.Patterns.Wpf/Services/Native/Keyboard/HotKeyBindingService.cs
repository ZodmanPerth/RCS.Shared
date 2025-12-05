using OKB.Logging;
using OKB.Utilities;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Windows.System;

#nullable disable

namespace OKB.Services.Native.Keyboard;

/// <inheritdoc cref="IHotKeyBindingService" />
public class HotKeyBindingService : IHotKeyBindingService, IDisposable
{

	readonly object _modifiersLock = new();

	readonly IThreadDispatcherService _threadDispatcher;

	// Dictionary of the hotkeys bound to an action
	readonly Dictionary<HotKey, Action> _bindings = new();

	// track of all non-modifier keys that are held down
	readonly HashSet<int> _activeKeysCodes = new();


	// track of all modifier keys that are being held down
	KeyModifiers _activeKeyModifiers;

	// Handle to the hook procedure to prevent garbage collection
	LowLevelKeyboardProc _hookProcedure;

	// The hook id.  Zero because it's optional.
	IntPtr _hookId = IntPtr.Zero;


	public bool IsLogKeyUpDown => false;
	public bool IsListening { get; private set; }



	//// Lifecycle


	public HotKeyBindingService(IThreadDispatcherService threadDispatcher)
	{
		_threadDispatcher = threadDispatcher ?? throw new ArgumentNullException(nameof(threadDispatcher));
	}

	public void Dispose()
	{
		StopListening();
		RemoveAllBindings();
	}



	//// Helpers


	IntPtr SetHook(LowLevelKeyboardProc proc)
	{
		var userLibrary = LoadLibrary("User32");
		return SetWindowsHookEx(WindowsHookEx.WH_KEYBOARD_LL, proc, userLibrary, 0);
	}

	IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
	{
		if (nCode < 0)
			return CallNextHookEx(_hookId, nCode, wParam, lParam);

		var keycode = Marshal.ReadInt32(lParam);

		// Check if the accumulation of keys is registered
		var isRegistered = IsAccumulatedHotKeyRegistered(wParam, keycode);
		if (isRegistered)
		{
			Logger.AsVerbose("Handled hot key binding")
				.WithProperty("modifiers", _activeKeyModifiers)
				.WithProperty("keycode", KeyInterop.KeyFromVirtualKey(keycode))
				.Write();

			// Set as handled
			return 1;
		}

		// Allow hook chain to continue processing
		return CallNextHookEx(_hookId, nCode, wParam, lParam);


		//// Local Functions


		/// <summary>True if the accumulated hot key is registered</summary>
		/// <remarks>
		/// The HookProcedure is called on individual key up/downs.
		/// We accumulate active key modifiers and key codes on down presses
		/// and deactive them on up presses.
		/// </remarks>
		bool IsAccumulatedHotKeyRegistered(IntPtr wParam, int keycode)
		{
			var modifierKey = KeyModifiersUtilities.AsKeyModifier(keycode);
			var isModifierKey = modifierKey != KeyModifiers.None;

			var triggerType = (WindowsMessage)wParam;

			// Check if triggered by a KeyDown (i.e. active)
			if (triggerType == WindowsMessage.KeyDown || triggerType == WindowsMessage.SystemKeyDown)
			{
				if (isModifierKey)
				{
					// Accumulate modifier key
					lock (_modifiersLock)
						_activeKeyModifiers |= modifierKey;

					if (IsLogKeyUpDown)
					{
						Logger.AsVerbose("Global key down")
							.WithProperty("modifierKey", modifierKey)
							.Write();
					}

					return false;
				}
				else
				{
					if (IsLogKeyUpDown)
					{
						Logger.AsVerbose("Global key down")
						.WithProperty("key", KeyInterop.KeyFromVirtualKey(keycode))
						.Write();
					}

					// Check if the keycode is already active
					if (_activeKeysCodes.Contains(keycode))
						return false;

					// Add key code
					_activeKeysCodes.Add(keycode);

					// Check if the accumulated hotkey is registered
					return InvokeRegisteredHotKeyAction((VirtualKey)keycode);
				}
			}

			// Check if triggered by a KeyUp (i.e. inactive)
			if (triggerType == WindowsMessage.KeyUp || triggerType == WindowsMessage.SystemKeyUp)
			{
				if (isModifierKey)
				{
					// Remove accumulated modifier key
					lock (_modifiersLock)
						_activeKeyModifiers ^= modifierKey;

					if (IsLogKeyUpDown)
					{
						Logger.AsVerbose("Global key up")
						.WithProperty("modifierKey", modifierKey)
						.Write();
					}

					return false;
				}
				else
				{
					if (IsLogKeyUpDown)
					{
						Logger.AsVerbose("Global key up")
						.WithProperty("key", KeyInterop.KeyFromVirtualKey(keycode))
						.Write();
					}

					// Remove accumulated keycode
					_activeKeysCodes.Remove(keycode);
				}

				return false;
			}

			// Ignore other triggers
			return false;
		}

		/// <summary>True if a registered hotkey action was invoked</summary>
		bool InvokeRegisteredHotKeyAction(VirtualKey virtualKey)
		{
			var activeHotKey = new HotKey(_activeKeyModifiers, virtualKey);

			var isActiveHotKeyRegistered = _bindings.ContainsKey(activeHotKey);
			if (!isActiveHotKeyRegistered)
				return false;

			// Get registered hotKey action
			if (!_bindings.TryGetValue(activeHotKey, out var registeredAction))
				return false;

			// Add action to thread pool for execution
			_threadDispatcher.QueueActionOnThreadPool((_) => InvokeBoundAction(registeredAction));

			return true;
		}

		void InvokeBoundAction(object actionObj)
		{
			var action = (Action)actionObj;
			action?.Invoke();
		}
	}



	//// Actions - registration


	public DisposableAction AddBinding(KeyModifiers modifiers, VirtualKey virtualKey, Action hotKeyAction)
	{
		if (hotKeyAction is null)
			return null;

		var hotkey = new HotKey(modifiers, virtualKey);
		_bindings[hotkey] = hotKeyAction;

		var unregisterDisposable = new DisposableAction(() => RemoveBinding(modifiers, virtualKey));
		return unregisterDisposable;
	}

	public void RemoveAllBindings() =>
		_bindings.Clear();

	public void RemoveBinding(KeyModifiers modifiers, VirtualKey virtualKey)
	{
		var keybind = new HotKey(modifiers, virtualKey);
		_bindings.Remove(keybind);
	}



	//// Actions


	public void StartListening()
	{
		if (IsListening)
			return;

		_hookProcedure = HookProcedure;
		_hookId = SetHook(_hookProcedure);

		IsListening = true;
	}

	public void StopListening()
	{
		if (!IsListening)
			return;

		UnhookWindowsHookEx(_hookId);

		IsListening = false;
	}



	//// Interop


	delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);


	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(WindowsHookEx idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhk);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

	[DllImport("kernel32.dll")]
	private static extern IntPtr LoadLibrary(string lpFileName);
}

