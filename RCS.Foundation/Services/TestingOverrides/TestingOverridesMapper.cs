using Riok.Mapperly.Abstractions;

namespace OKB.Models.TestingOverrides;

[Mapper]
public static partial class TestingOverridesMapper
{
	/// <summary>Copies the properties of <paramref name="source"/> to the <paramref name="target"/></summary>
	public static partial void UpdateTargetFromSource(TestingOverrides source, TestingOverrides target);
}

