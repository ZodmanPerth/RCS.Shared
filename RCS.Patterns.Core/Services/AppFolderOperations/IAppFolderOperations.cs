namespace OKB.Services;

public interface IAppFolderOperations
{
	/// <summary>Reads the contents of a text file in the app folder store</summary>
	string ReadTextFile(string fileName, AppFolderStore appFolderStore = AppFolderStore.Root);

	/// <summary>Writes text as the contents of the passed file in the app folder store</summary>
	void WriteTextFile(string fileName, string text, AppFolderStore appFolderStore = AppFolderStore.Root);
}