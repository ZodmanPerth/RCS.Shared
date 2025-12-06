namespace RCS.Repositories;

/// <summary>The model <typeparamref name="T"/> is a class whose values are updatable from another instance of <typeparamref name="T"/></summary>
public interface IModelUpdatable<T> where T : class
{
	/// <summary>Update the properties of the current instance from another source</summary>
	void UpdateFrom(T source);
}
