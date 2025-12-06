using RCS.Windows.PersistedState;
using System.ComponentModel;

#nullable disable

namespace RCS.Models;

public interface IAppSettingsNative : INotifyPropertyChanged
{
	/// <summary>The state of all persisted windows by name</summary>
	Dictionary<string, PersistedWindowState> PersistedWindowStates { get; set; }

	/// <summary>Fired when a dictionary is updated</summary>
	void PersistedWindowStatesValueUpdated();
}
