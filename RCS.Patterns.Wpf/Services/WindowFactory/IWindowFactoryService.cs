using RCS.Windows;
using System.Windows.Controls;

namespace RCS.Services.WindowFactory;

public interface IWindowFactoryService
{
	/// <summary>Returns a window containting a composed view</summary>
	WindowBase CreateAndCompose(Func<WindowBase> createWindow, Func<UserControl> createView);

	/// <summary>Returns a window with a pre-composed view</summary>
	WindowBase CreateAndCompose(Func<WindowBase> createWindow, UserControl view);

	/// <summary>Returns a window with a composed view that can do curated operations on the window</summary>
	WindowBase CreateAndCompose(Func<WindowBase> createWindow, Func<ViewWindowOperations, UserControl> createView);
}
