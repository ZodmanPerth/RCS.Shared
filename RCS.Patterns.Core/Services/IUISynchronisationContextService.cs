namespace RCS.Services;
public interface IUISynchronisationContextService
{
	SynchronizationContext UISynchronisationContext { get; }
}