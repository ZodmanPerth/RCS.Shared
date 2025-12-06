namespace RCS.Services;

public class AppFolderOperations : IAppFolderOperations
{
	public const string RootAppFolderPath = "RCS";

	string _rootAppFolderPath;

	object lockObject = new();


	//// Lifecycle


	public AppFolderOperations()
	{
		_rootAppFolderPath = Path.Combine
		(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			RootAppFolderPath
		);
	}



	//// Helpers


	void EnsureRootAppFolderExists()
	{
		if (Directory.Exists(_rootAppFolderPath))
			return;

		Directory.CreateDirectory(_rootAppFolderPath);
	}

	string GetFilePath(AppFolderStore appFolderStore, string fileName) =>
		appFolderStore switch
		{
			AppFolderStore.Root or _ => Path.Combine(_rootAppFolderPath, fileName)
		};



	//// Actions


	public string ReadTextFile(string fileName, AppFolderStore appFolderStore = AppFolderStore.Root)
	{
		lock (lockObject)
		{
			EnsureRootAppFolderExists();

			var filePath = GetFilePath(appFolderStore, fileName);

			if (!File.Exists(filePath))
				return string.Empty;

			return File.ReadAllText(filePath);
		}
	}

	public void WriteTextFile(string fileName, string text, AppFolderStore appFolderStore = AppFolderStore.Root)
	{
		lock (lockObject)
		{
			EnsureRootAppFolderExists();

			var filePath = GetFilePath(appFolderStore, fileName);

			// Ensure the relative file path exists
			var folderPath = Path.GetDirectoryName(filePath);
			Directory.CreateDirectory(folderPath!);

			File.WriteAllText(filePath, text);
		}
	}
}
