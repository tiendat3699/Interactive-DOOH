using GadGame.SO;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace GadGame.Editor.Window
{
    public class ConfigWindow : OdinMenuEditorWindow
    {
        [MenuItem("Game Config/Open Window")]
        private static void ShowWindow()
        {
            GetWindow<ConfigWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("Scene Flow", SceneFlowConfig.GetOrCreateSettings());
            tree.Add("Sound", AudioConfig.GetOrCreateSettings());

            return tree;
        }
    }
}