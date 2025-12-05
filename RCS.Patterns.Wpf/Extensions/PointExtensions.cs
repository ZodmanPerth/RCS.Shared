namespace System.Windows;

public static class PointExtensions
{
	/// <summary>Returns the point translated by x and y from the original point</summary>
	public static Point TranslatedBy(this Point point, double x = 0, double y = 0) =>
		new Point(point.X + x, point.Y + y);

}

