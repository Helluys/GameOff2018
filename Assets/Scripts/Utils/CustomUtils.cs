using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUtils {

    /// <summary>
    /// Convert screen position to canvas position
    /// </summary>
    /// <param name="canvas">Canvas use for the conversion</param>
    /// <returns>Converted screen position</returns>
	public static Vector3 ConvertToCanvasPos(Canvas canvas,Vector2 position)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle
            (canvas.transform as RectTransform, position, canvas.worldCamera, out pos);
        pos.z = 0;
        return pos;
    }
    /// <summary>
    /// Check if the screen position is contained in a rectangle
    /// </summary>
    /// <param name="rect">RectTransform of interest</param>
    /// <returns>Whether the screen position is contained in rect</returns>
    public static bool IsPosInRect (RectTransform rect,Vector2 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, position);
    }
    /// <summary>
    /// Remap a value within a new range
    /// </summary>
    /// <param name="val">Value to remap</param>
    /// <param name="oldMin">Original low bound</param>
    /// <param name="oldMax">Original high bound</param>
    /// <param name="newMin">New low bound</param>
    /// <param name="newMax">New high bound</param>
    /// <returns>Rempad value</returns>
    public static float Remap(float val, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (val - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }
    /// <summary>
    /// Convert a value in seconds into a time formated string
    /// </summary>
    /// <param name="value">Value in second to convert</param>
    /// <returns>Value converted to string</returns>
    public static string ConvertToTimeFormat(float value)
    {
        string minutes = ((int)value / 60).ToString("00");
        string seconds = (value % 60).ToString("00.00");
        return minutes + ":" + seconds;
    }
}
