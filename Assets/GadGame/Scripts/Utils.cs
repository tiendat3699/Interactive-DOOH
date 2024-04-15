using UnityEngine;

namespace GadGame
{
    public static class Utils
    {
        public static Vector2 RandomPointInside(this Rect rect)
        {
            var rectSize = rect.size;
            return rect.position + new Vector2(Random.Range(0, rectSize.x), Random.Range(0, rectSize.y));
        }
    }
}