using RCS.Utilities;

namespace RCS.Services.Native.Keyboard;

public static class KeyModifiersUtilities
{
	public static KeyModifiers AsKeyModifier(int keyCode)
	{
		switch (keyCode)
		{
			case (int)VirtualKeys.Shift:
			case (int)VirtualKeys.LeftShift:
			case (int)VirtualKeys.RightShift:
				return KeyModifiers.Shift;

			case (int)VirtualKeys.Control:
			case (int)VirtualKeys.LeftControl:
			case (int)VirtualKeys.RightControl:
				return KeyModifiers.Control;

			case (int)VirtualKeys.Alt:
			case (int)VirtualKeys.LeftAlt:
			case (int)VirtualKeys.RightAlt:
				return KeyModifiers.Alt;

			case (int)VirtualKeys.LeftWindows:
			case (int)VirtualKeys.RightWindows:
				return KeyModifiers.WindowsKey;

			default:
				return KeyModifiers.None;
		}
	}
}