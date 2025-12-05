namespace OKB.Core.Types;

public static class CoreTypeExtensions
{
	public static (string openBrace, string closeBrace) GetBraces(this BraceType braceType) => braceType switch
	{
		BraceType.Round => ("(", ")"),
		BraceType.Square => ("[", "]"),
		BraceType.Curly => ("{", "}"),
		_ => ("", "")
	};
}

