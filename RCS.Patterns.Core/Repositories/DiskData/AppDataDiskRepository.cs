using OKB.Services;
using OKB.Utilities;
using System.ComponentModel;

namespace OKB.Repositories.DiskData;

/// <inheritdoc cref="IMonitoredModelRepository" />
/// <remarks>This repository uses the AppData folder on disk as a persistent store.</remarks>
public class AppDataDiskRepository : IDisposable, IMonitoredModelRepository
{
	readonly IAppFolderOperations _appFolderOperations;
	readonly ThrottledActionFactory _throttledActionFactory;

	List<IModelFilePersistenceManager> _monitoredModels = new();



	//// Lifecycle


	public AppDataDiskRepository
	(
		// services
		IAppFolderOperations appFolderOperations,

		// factories
		ThrottledActionFactory throttledActionFactory
	)
	{
		_appFolderOperations = appFolderOperations ?? throw new ArgumentNullException(nameof(appFolderOperations));

		_throttledActionFactory = throttledActionFactory ?? throw new ArgumentNullException(nameof(throttledActionFactory));
	}

	public void Dispose()
	{
		foreach (var modelFileManager in _monitoredModels)
		{
			modelFileManager?.SaveFile();
			modelFileManager?.Dispose();
		}

		_monitoredModels.Clear();
	}


	//// IMonitoredModelRepository


	public ThrottledAction RegisterThrottledAutoSync<TModel>(TModel model, ResourceType type)
		where TModel : class, INotifyPropertyChanged, IModelUpdatable<TModel>
	{
		if (model is null) throw new ArgumentNullException(nameof(model));
		if (!Enum.IsDefined(type)) throw new ArgumentOutOfRangeException(nameof(type)); // Carl TODO: update snippet


		// Create a new ModelFilePersistenceManager to manage the model
		var manager = new ModelFilePersistenceManager<TModel>
		(
			_throttledActionFactory,
			_appFolderOperations,
			model,
			GetRelativeFilePath()
		);

		_monitoredModels.Add(manager);

		return manager.SaveModelToFileThrottledAction;


		//// Local Functions


		string GetRelativeFilePath()
		{
			var relativeFilePath = type.ToRelativeResourcePath();

			// Add a ".json" extension because this repository persists the models as json files to disk
			if (!relativeFilePath!.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
				relativeFilePath += ".json";

			return relativeFilePath;
		}
	}
}
