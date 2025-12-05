using System.Windows;

#nullable disable

namespace OKB.Windows.PersistedState;

public class PersistedWindowState
{
	public double Top { get; set; }
	public double Left { get; set; }
	public double Width { get; set; }
	public double Height { get; set; }
	public WindowState State { get; set; }



	//// Lifecycle


	public PersistedWindowState() { /* nada */ }

	public PersistedWindowState(PersistedWindowState data)
	{
		if (data is null)
			return;

		Top = data.Top;
		Left = data.Left;
		Width = data.Width;
		Height = data.Height;
		State = data.State;
	}



	//// Actions


	public void ApplyRect(Rect rect)
	{
		Top = rect.Top;
		Left = rect.Left;
		Width = rect.Width;
		Height = rect.Height;
	}

	public void ApplyState(PersistedWindowState newState)
	{
		Top = newState.Top;
		Left = newState.Left;
		Width = newState.Width;
		Height = newState.Height;
		State = newState.State;
	}

	public Rect AsRect() =>
		new Rect(Left, Top, Width, Height);

	public override string ToString() =>
		$"{AsRect()} {State}";
}
