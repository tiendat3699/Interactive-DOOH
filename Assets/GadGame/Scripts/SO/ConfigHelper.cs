using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ConfigHelper
{
    public static T GetConfig<T>() where T : Config {
        return Resources.Load<T>($"GameConfig/{typeof(T).Name}");
    }

#if UNITY_EDITOR
    public static T GetOrCreateConfig<T>() where T : Config {
        var config = GetConfig<T>();
        if (config == null)
        {
            config = ScriptableObject.CreateInstance<T>();
            string path = "Assets/Resources/GameConfig/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(config, $"{path}/{typeof(T).Name}.asset");
            AssetDatabase.SaveAssets();
        }

        return config;
    }
#endif
}
