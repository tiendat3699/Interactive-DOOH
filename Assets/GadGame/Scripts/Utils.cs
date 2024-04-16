using UnityEngine;

namespace GadGame
{
    public static class Utils
    {
        public static Vector2 RandomPointInside(this Rect rect)
        {
            var rectSize = rect.size;
            var bottomLeft = rect.position + new Vector2(-rect.width / 2, -rect.height / 2);
            return bottomLeft + new Vector2(Random.Range(0, rectSize.x), Random.Range(0, rectSize.y));
        }
    }
}