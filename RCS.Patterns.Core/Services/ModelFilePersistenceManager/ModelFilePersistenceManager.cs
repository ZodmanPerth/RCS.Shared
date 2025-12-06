using RCS.Repositories;
using RCS.Utilities;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable disable

namespace RCS.Services;

// Kudos: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-6-0
/// <summary>
/// Manages the load/save of a <typeparamref name="TModel"/> from/to the passed filename in the
/// application's AppData Folder.  Saves occur automatically whenever a property of the model changes
/// via <see cref="INotifyPropertyChanged"/>.  Saves are throttled to avoid disk thrashing.<br></br>
/// </summary>
public class ModelFilePersistenceManager<TModel> : IDisposable, IModelFilePersistenceManager
	where TModel : class, INotifyPropertyChanged, IModelUpdatable<TModel>
{
	readonly IAppFolderOperations _appFolderOperations;

	readonly string _filename;

	readonly JsonSerializerOptions _serialiserOptions;

	TModel _model;


	/// <summary>The action that performs the save after a delay</summary>
	public ThrottledAction SaveModelToFileThrottledAction { get; }



	//// Lifecycle


	public ModelFilePersistenceManager
	(
		ThrottledActionFactory throttledActionFactory,
		IAppFolderOperations appFolderOperations,
		TModel model,
		string filename
	)
	{
		if (throttledActionFactory is null) throw new ArgumentNullException(nameof(throttledActionFactory));
		SaveModelToFileThrottledAction = throttledActionFactory.CreateToRunOnBackgroundThreads
		(
			SaveFile,
			TimeSpan.FromSeconds(1)
		);

		_appFolderOperations = appFolderOperations ?? throw new ArgumentNullException(nameof(appFolderOperations));
		_model = model ?? throw new ArgumentNullException(nameof(model));
		_filename = filename ?? throw new ArgumentNullException(nameof(filename));

		_serialiserOptions = new JsonSerializerOptions()
		{
			AllowTrailingCommas = true,
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		};

		// Initialise model from file
		if (!LoadModelFromFile())
			SaveFile();     // save new file using the default model settings if the file couldn't be loaded

		_model.PropertyChanged += (s, e) =>
			SaveModelToFileThrottledAction.Invoke();
	}

	public void Dispose()
	{
		SaveModelToFileThrottledAction.Dispose();
	}



	//// Helpers


	/// <summary>true if model was loaded from file</summary>
	bool LoadModelFromFile()
	{
		var json = _appFolderOperations.ReadTextFile(_filename);
		if (json.IsNullOrWhitespace())
			return false;

		try
		{
			var loadedData = JsonSerializer.Deserialize<TModel>(json, _serialiserOptions);
			_model.UpdateFrom(loadedData);
			return true;
		}
		catch
		{
			return false;
		}
	}



	//// Actions


	public void SaveFile()
	{
		var json = JsonSerializer.Serialize(_model, options: _serialiserOptions);
		_appFolderOperations.WriteTextFile(_filename, json);
	}
}
