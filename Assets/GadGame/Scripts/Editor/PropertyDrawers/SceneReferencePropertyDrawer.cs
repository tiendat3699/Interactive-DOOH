using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GadGame.Scripts.Editor.PropertyDrawers
{
    [UnityEditor.CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var isDirtyProperty = property.FindPropertyRelative("_isDirty");
			if (isDirtyProperty.boolValue) {
				isDirtyProperty.boolValue = false;
				// This will force change in the property and make it dirty.
				// After the user saves, he'll actually see the changed changes and commit them.
			}

			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			const float buildSettingsWidth = 20f;
			const float padding = 2f;

			Rect assetPos = position;
			assetPos.width -= buildSettingsWidth + padding;

			Rect buildSettingsPos = position;
			buildSettingsPos.x += position.width - buildSettingsWidth + padding;
			buildSettingsPos.width = buildSettingsWidth;

			var sceneAssetProperty = property.FindPropertyRelative("_sceneAsset");
			bool hadReference = sceneAssetProperty.objectReferenceValue != null;

			EditorGUI.PropertyField(assetPos, sceneAssetProperty, new GUIContent());

			int indexInSettings = -1;

			if (sceneAssetProperty.objectReferenceValue) {
				if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sceneAssetProperty.objectReferenceValue, out var guid, out long _)) {
					indexInSettings = Array.FindIndex(EditorBuildSettings.scenes, s => s.guid.ToString() == guid);
				}
			} else if (hadReference) {
				property.FindPropertyRelative("_scenePath").stringValue = string.Empty;
			}

			GUIContent settingsContent = indexInSettings != -1
				? new GUIContent("-", "Scene is already in the Editor Build Settings. Click here to remove it.")
				: new GUIContent("+", "Scene is missing in the Editor Build Settings. Click here to add it.")
				;

			Color prevBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = indexInSettings != -1 ? Color.red : Color.green;

			if (GUI.Button(buildSettingsPos, settingsContent, EditorStyles.miniButtonRight) && sceneAssetProperty.objectReferenceValue) {
				if (indexInSettings != -1) {
					var scenes = EditorBuildSettings.scenes.ToList();
					scenes.RemoveAt(indexInSettings);

					EditorBuildSettings.scenes = scenes.ToArray();

				} else {
					var newScenes = new[] {
						new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(sceneAssetProperty.objectReferenceValue), true)
					};

					EditorBuildSettings.scenes = EditorBuildSettings.scenes.Concat(newScenes).ToArray();
				}
			}

			GUI.backgroundColor = prevBackgroundColor;

			EditorGUI.EndProperty();
		}
    }
}