using OKB.Utilities;
using Windows.System;

namespace OKB.Services.Native.Keyboard;

public class HotKey
{
	public KeyModifiers Modifiers { get; }
	public VirtualKey VirtualKey { get; }

	public HotKey(KeyModifiers modifiers, VirtualKey virtualKey)
	{
		VirtualKey = virtualKey;
		Modifiers = modifiers;
	}

	public bool Equals(HotKey other)
	{
		if (other is null)
			return false;

		if (Modifiers != other.Modifiers)
			return false;

		if (VirtualKey != other.VirtualKey)
			return false;

		return true;
	}

	public override bool Equals(object? other)
	{
		if (other is null)
			return false;

		if (other.GetType() != GetType())
			return false;

		return Equals((HotKey)other);
	}

	public override int GetHashCode()
	{
		var modifierFactor = (int)KeyModifiers.AllModifiers + 1;
		var hash = VirtualKey.GetHashCode() * modifierFactor + Modifiers.GetHashCode();
		return hash;
	}
}
