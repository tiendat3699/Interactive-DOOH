using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace GadGame.SO
{
    [CreateAssetMenu]
    public class SceneFlowConfig : ScriptableObject
    {
        public SceneReference IdleScene;
        public SceneReference PassByScene;
        public SceneReference ViewedScene;
        public SceneReference EndGageScene;
        public SceneReference GameScene;
        public SceneReference CTAScene;


#if UNITY_EDITOR
        public static SceneFlowConfig FindSettings()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(SceneFlowConfig)}");
            if (guids.Length > 1) Debug.LogWarning("Found multiple settings files, using the first.");

            switch (guids.Length)
            {
                case 0:
                    return null;
                default:
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    return AssetDatabase.LoadAssetAtPath<SceneFlowConfig>(path);
            }
        }
       
        public static SceneFlowConfig GetOrCreateSettings()
        {
            var settings = FindSettings();
            if (settings == null)
            {
                settings = CreateInstance<SceneFlowConfig>();
                string path = "Assets/GadGame/SO/SceneFlowConfig";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(settings, $"{path}/Scene Flow Config.asset");
                AssetDatabase.SaveAssets();
            }

            return settings;
        }
#endif
    }
}