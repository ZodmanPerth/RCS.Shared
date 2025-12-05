namespace OKB.Services;

public class UISynchronisationContextService : IUISynchronisationContextService
{
	public SynchronizationContext UISynchronisationContext { get; }

	public UISynchronisationContextService(SynchronizationContext uiSynchronisationContext) =>
		UISynchronisationContext = uiSynchronisationContext ?? throw new ArgumentNullException(nameof(uiSynchronisationContext));
}

