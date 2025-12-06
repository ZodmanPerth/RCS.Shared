#nullable disable

namespace RCS.Models;

public class ClipboardItem
{
	Uri _uri = null;


	public string Text { get; }

	/// <summary>The index into the containing collection</summary>
	public int Index { get; }


	/// <summary>True when the text is just regular text</summary>
	public bool IsRegularText => !IsInteger && !IsUri;

	/// <summary>True when <see cref="Text"/> is an integer</summary>
	public bool IsInteger { get; private set; }

	/// <summary>True when <see cref="Text"/> is a Uri</summary>
	public bool IsUri => _uri is not null && (_uri.Scheme == Uri.UriSchemeHttp || _uri.Scheme == Uri.UriSchemeHttps);



	//// Lifecycle


	public ClipboardItem(string text, int index)
	{
		Text = text;
		Index = index;

		ParseText();

		return;


		//// Local Functions


		void ParseText()
		{
			IsInteger = int.TryParse(text, out var integer);

			Uri.TryCreate(Text, UriKind.Absolute, out _uri);
		}
	}
}