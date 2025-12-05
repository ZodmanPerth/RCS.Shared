namespace OKB.Services;
public interface IUISynchronisationContextService
{
	SynchronizationContext UISynchronisationContext { get; }
}