using System.Windows.Controls;
using System.Windows.Documents;

namespace RCS.Components;

/// <summary>Wraps the text of a textblock in nonbreaking spaces</summary>
/// <remarks>
/// When used in a Run, surround these in <see cref="InlineUIContainer"/> to prevent issues wrapping
/// on non-breaking spaces.
/// </remarks>
public class CodeBlock : TextBlock
{
	public CodeBlock()
	{
		Loaded += (s, e) =>
		{
			if (string.IsNullOrWhiteSpace(Text))
				return;

			// NOTE:
			// In WPF you can only insert special characters into XAML, not escapes strings.
			// We wrap our text in NBSP characters to prevent collapsing spaces when wrapping a code run.
			Text = $"\u00A0{Text}\u00A0";
		};
	}
}
