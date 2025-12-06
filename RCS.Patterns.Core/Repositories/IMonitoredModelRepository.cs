using RCS.Utilities;
using System.ComponentModel;

namespace RCS.Repositories;

/// <summary>
/// The repository supports keeping a model in memory and persisting whenever a property changes.<br></br>
/// The model is initialised from the repository at registration time and persisted whenever a property changes.<br></br>
/// Persistence to the repository may be eventually consistent - saves will be throttled if required by the repository.
/// </summary>
public interface IMonitoredModelRepository : IDisposable
{
	/// <summary>Returns a throttled action that syncs the model to/from a repository</summary>
	/// <remarks>Saves to the repository may be eventually consistent, based on the repository</remarks>
	ThrottledAction RegisterThrottledAutoSync<TModel>(TModel model, ResourceType type)
		where TModel : class, INotifyPropertyChanged, IModelUpdatable<TModel>;
}