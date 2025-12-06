namespace RCS.Utilities;

[Flags]
public enum KeyModifiers
{
	None = 0,
	Alt = 1,
	Control = 2,
	Shift = 4,
	WindowsKey = 8,

	AllModifiers = Alt | Control | Shift | WindowsKey,
}
