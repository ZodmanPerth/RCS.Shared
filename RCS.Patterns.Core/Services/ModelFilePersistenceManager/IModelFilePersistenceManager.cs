namespace OKB.Services;

/// <summary>A simple interface for the generic <see cref="ModelFilePersistenceManager{TModel}"/></summary>
public interface IModelFilePersistenceManager : IDisposable
{
	/// <summary>Immediately save the file</summary>
	void SaveFile();
}