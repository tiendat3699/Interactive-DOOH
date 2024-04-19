using System.Collections.Generic;
using System.Linq;
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
        
        public static string GetTemplate(string templateName, Dictionary<string, string> arguments)
        {
            var filePath = $"Templates/{templateName}";
            var textAsset = Resources.Load<TextAsset>(filePath);
            var template = textAsset.text;
            Resources.UnloadAsset(textAsset);
            return arguments.Aggregate(template, (current, argument) => current.Replace($"$[{argument.Key}]", argument.Value));
        }
    }
}