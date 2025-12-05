using OKB.Windows;
using System.Windows.Controls;

namespace OKB.Services.WindowFactory;

public class WindowFactoryService : IWindowFactoryService
{
	public WindowBase CreateAndCompose(Func<WindowBase> createWindow, Func<UserControl> createView)
	{
		var window = createWindow();
		var view = createView();

		window.Content = view;
		return window;
	}

	public WindowBase CreateAndCompose(Func<WindowBase> createWindow, UserControl view)
	{
		var window = createWindow();

		window.Content = view;
		return window;
	}

	public WindowBase CreateAndCompose(Func<WindowBase> createWindow, Func<ViewWindowOperations, UserControl> createView)
	{
		var window = createWindow();

		var windowCloseAction = new ViewWindowOperations(window.Close);
		var view = createView(windowCloseAction);

		window.Content = view;
		return window;
	}
}
