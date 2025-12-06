using Riok.Mapperly.Abstractions;

namespace RCS.Models.TestingOverrides;

[Mapper]
public static partial class TestingOverridesMapper
{
	/// <summary>Copies the properties of <paramref name="source"/> to the <paramref name="target"/></summary>
	public static partial void UpdateTargetFromSource(TestingOverrides source, TestingOverrides target);
}

