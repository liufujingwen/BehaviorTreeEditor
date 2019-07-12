using UnityEngine;
using System.Collections;

public static class RectExtension {
	public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
	{
		Rect rect1 = rect;
		rect1.x = rect1.x - pivotPoint.x;
		rect1.y = rect1.y - pivotPoint.y;
		rect1.xMin = rect1.xMin * scale;
		rect1.xMax = rect1.xMax * scale;
		rect1.yMin = rect1.yMin * scale;
		rect1.yMax = rect1.yMax * scale;
		rect1.x = rect1.x + pivotPoint.x;
		rect1.y = rect1.y + pivotPoint.y;
		return rect1;
	}

	public static Vector2 TopLeft(this Rect r)
	{
		return new Vector2(r.x, r.y);
	}
	
	public static Vector2 TopRight(this Rect r)
	{
		return new Vector2(r.xMax, r.y);
	}
}
