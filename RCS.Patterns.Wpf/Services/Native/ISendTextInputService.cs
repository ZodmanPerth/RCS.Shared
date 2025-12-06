namespace RCS.Services.Native;

public interface ISendTextInputService
{
	/// <summary>Sends the text to the active window as a sequence of key down/up events</summary>
	void SendUnicodeTextAsKeystrokes(string unicodeText);
}