namespace OKB.Repositories;

public static class RepositoryExtensions
{
	const string ViewsBasePath = "Views";


	/// <summary>Converts the resource type to a relative resource path</summary>
	public static string ToRelativeResourcePath(this ResourceType type) => type switch
	{
		ResourceType.AppSettings => "settings",
		ResourceType.TestingOverrides => "testingOverrides",

		ResourceType.LivingStyleGuideViewSettings => $"{ViewsBasePath}/LivingStyleGuide",

		_ => throw new NotSupportedException($"The resource type '{type}' is not supported"),
	};
}
