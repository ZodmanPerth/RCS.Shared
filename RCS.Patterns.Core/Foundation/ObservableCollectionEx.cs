using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace RCS.Patterns;

public class ObservableCollectionEx<T> : ObservableCollection<T>
{
	//// Lifecycle

	public ObservableCollectionEx() : base() { /* nada */ }
	public ObservableCollectionEx(IEnumerable<T> collection) : base(collection) { /* nada */ }
	public ObservableCollectionEx(List<T> list) : base(list) { /* nada */ }



	//// Actions


	/// <summary>Adds a range of items, only firing propertychanged events once</summary>
	public void AddRange(IEnumerable<T> items)
	{
		if (items is null)
			return;

		var newItems = new List<T>();
		using (BlockReentrancy())
		{
			var index = Count;

			foreach (var item in items)
			{
				Items.Add(item);
				newItems.Add(item);
			}

			if (newItems.Count == 0) // Count is now known
				return;

			// Notify about new items
			var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, index);
			OnCollectionChanged(args);

			// Notify about new count
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
		}
	}

	/// <summary>Replaces all content with the passed items, only firing propertychanged events once</summary>
	public void ResetContent(IEnumerable<T> items)
	{
		if (items is null)
			return;

		var newItems = new List<T>();
		using (BlockReentrancy())
		{
			var oldCount = Count;

			Items.Clear();

			foreach (var item in items)
			{
				Items.Add(item);
				newItems.Add(item);
			}

			// Notify about change in content
			var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			OnCollectionChanged(args);

			// Notify about new count (if required)
			if (oldCount != Count)
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
		};
	}

	/// <summary>Inserts a range of items at index, only firing property changed events once</summary>
	public void Insert(int index, IEnumerable<T> items)
	{
		if (items is null)
			return;

		var newItems = new List<T>();
		var dynamicIndex = index;
		using (BlockReentrancy())
		{

			foreach (var item in items)
			{
				Items.Insert(dynamicIndex++, item);
				newItems.Add(item);
			}

			if (newItems.Count == 0) // Count is now known
				return;

			// Notify about new items
			var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, index);
			OnCollectionChanged(args);

			// Notify about new count
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
		}
	}
}
