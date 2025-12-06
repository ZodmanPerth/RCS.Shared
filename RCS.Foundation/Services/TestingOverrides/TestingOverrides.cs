using OKB.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace OKB.Models.TestingOverrides;

/// <inheritdoc cref="ITestingOverrides" />
public partial class TestingOverrides : ObservableObject, ITestingOverrides, IModelUpdatable<TestingOverrides>
{
	[ObservableProperty]
	[Description("Allow all navigation keys to be valid")]
	[Category("Testing")]
	public partial bool AllowInvalidNavigationKeys { get; set; } = false;



	//// Actions


	public void UpdateFrom(TestingOverrides source) =>
		TestingOverridesMapper.UpdateTargetFromSource(source, this);
}