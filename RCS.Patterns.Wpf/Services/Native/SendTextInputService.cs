using System.Runtime.InteropServices;

namespace RCS.Services.Native;

/// <summary>Sends input to the active window</summary>
// Kudos: https://stackoverflow.com/a/8885228/117797
public class SendTextInputService : ISendTextInputService
{
	public void SendUnicodeTextAsKeystrokes(string unicodeText)
	{
		// Create key down/up events for character in the string
		var keyboardEventInputs = unicodeText
			.SelectMany(_ => CreateUnicodeKeyStrokeEvents(_))
			.ToArray();

		// Send keyboard events in a single call
		SendInput
		(
			(uint)keyboardEventInputs.Length,
			keyboardEventInputs,
			Marshal.SizeOf(typeof(Input))
		);

		return;


		//// Local Functions


		/// <summary>Returns a sequence of key down/up events for a unicode character</summary>
		IEnumerable<Input> CreateUnicodeKeyStrokeEvents(char unicodeCharacter, bool isCreateKeyDownEvent = true, bool isCreateKeyUpEvent = true)
		{
			if (isCreateKeyDownEvent)
				yield return CreateKeystrokeInputEvent(unicodeCharacter, isKeyUp: false);

			if (isCreateKeyUpEvent)
				yield return CreateKeystrokeInputEvent(unicodeCharacter, isKeyUp: true);

			yield break;


			//// Local Functions


			/// <summary>Creates an input event for a single keystroke action (key down or key up)</summary>
			Input CreateKeystrokeInputEvent(char unicodeCharacter, bool isKeyUp)
			{
				var keystrokeInput = new KeyboardInput
				{
					wVk = 0,    // must be 0 since we are sending Unicode characters.
					wScan = unicodeCharacter,
					dwFlags = (uint)(InputDwFlags.KEYEVENTF_UNICODE | (isKeyUp ? InputDwFlags.KEYEVENTF_KEYUP : 0)),
					dwExtraInfo = GetMessageExtraInfo(),
				};

				var inputUnion = new InputUnion() { ki = keystrokeInput };

				return new Input()
				{
					type = (int)InputTypes.INPUT_KEYBOARD,
					u = inputUnion
				};
			}
		}
	}




	//// Imports


	[DllImport("user32.dll")]
	static extern nint GetMessageExtraInfo();

	[DllImport("user32.dll", SetLastError = true)]
	static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);
}
