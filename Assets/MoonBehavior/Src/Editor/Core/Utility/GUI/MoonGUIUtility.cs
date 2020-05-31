using UnityEditor;
using UnityEngine;

namespace MoonBehaviorEditor
{
    /// <summary>
    /// GUI Utility  
    /// </summary>
    public static class MoonGUIUtility
    {
        /// <summary>
        /// Gets rect based on 2 positions
        /// </summary>
        /// <param name="screenPosition1">Position 1</param>
        /// <param name="screenPosition2">Position 2</param>
        /// <returns>Rect</returns>
        public static Rect GetScreenRect(Vector2 screenPosition1, Vector2 screenPosition2)
        {
            Vector2 topLeft = Vector2.Min(screenPosition1, screenPosition2);
            Vector2 bottomRight = Vector2.Max(screenPosition1, screenPosition2);
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
    }
}