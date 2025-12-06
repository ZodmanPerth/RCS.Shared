namespace RCS.Models.TestingOverrides;

/// <summary>Values that change the behaviour of the app in debug/testing builds</summary>
public interface ITestingOverrides
{
	/// <summary>true to allow any navigation keys to be accepted</summary>
	/// <remarks>Used to debug the visualisation of keys in the Navigation UI</remarks>
	bool AllowInvalidNavigationKeys { get; set; }
}