using GadGame.SO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GadGame.Editor.Window
{
    public class SceneFlowConfigWindow : OdinEditorWindow
    {
        [MenuItem("Game Config/Scene Flow")]
        private static void ShowWindow()
        {
            var window = InspectObject(SceneFlowConfig.GetOrCreateSettings());
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 200);
            window.maxSize = new Vector2(600, 200);
            window.minSize = new Vector2(600, 200);
        }
    }
}