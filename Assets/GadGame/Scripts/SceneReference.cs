using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace GadGame
{
	[Serializable]
	public class SceneReference : ISerializationCallbackReceiver
	{
#if UNITY_EDITOR
		// Reference to the asset used in the editor. Player builds don't know about SceneAsset.
		// Will be used to update the scene path.
		[SerializeField] private SceneAsset _sceneAsset;

#pragma warning disable 0414 // Never used warning - will be used by SerializedProperty.
		// Used to dirtify the data when needed upon displaying in the inspector.
		// Otherwise the user will never get the changes to save (unless he changes any other field of the object / scene).
		[SerializeField] private bool _isDirty;
#pragma warning restore 0414
#endif

		// Player builds will use the path stored here. Should be updated in the editor or during build.
		// If scene is deleted, path will remain.
		[SerializeField] private string _scenePath = string.Empty;


		/// <summary>
		/// Returns the scene path to be used in the <see cref="UnityEngine.SceneManagement.SceneManager"/> API.
		/// While in the editor, this path will always be up to date (if asset was moved or renamed).
		/// If the referred scene asset was deleted, the path will remain as is.
		/// </summary>
		public string ScenePath
		{
			get
			{
#if UNITY_EDITOR
				AutoUpdateReference();
#endif

				return _scenePath;
			}

			set
			{
				_scenePath = value;

#if UNITY_EDITOR
				if (string.IsNullOrEmpty(_scenePath))
				{
					_sceneAsset = null;
					return;
				}

				_sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(_scenePath);
				if (_sceneAsset == null)
				{
					Debug.LogError(
						$"Setting {nameof(SceneReference)} to {value}, but no scene could be located there.");
				}
#endif
			}
		}

		/// <summary>
		/// Returns the name of the scene without the extension.
		/// </summary>
		public string SceneName => Path.GetFileNameWithoutExtension(ScenePath);

		public bool IsEmpty => string.IsNullOrEmpty(ScenePath);

		public SceneReference()
		{
		}

		public SceneReference(string scenePath)
		{
			ScenePath = scenePath;
		}

		public SceneReference(SceneReference other)
		{
			_scenePath = other._scenePath;

#if UNITY_EDITOR
			_sceneAsset = other._sceneAsset;
			_isDirty = other._isDirty;

			AutoUpdateReference();
#endif
		}

		public SceneReference Clone() => new SceneReference(this);

		public override string ToString()
		{
			return _scenePath;
		}

		[Obsolete("Needed for the editor, don't use it in runtime code!", true)]
		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			AutoUpdateReference();
#endif
		}

		[Obsolete("Needed for the editor, don't use it in runtime code!", true)]
		public void OnAfterDeserialize()
		{
#if UNITY_EDITOR
			// OnAfterDeserialize is called in the deserialization thread so we can't touch Unity API.
			// Wait for the next update frame to do it.
			EditorApplication.update += OnAfterDeserializeHandler;
#endif
		}


#if UNITY_EDITOR
		private void OnAfterDeserializeHandler()
		{
			EditorApplication.update -= OnAfterDeserializeHandler;
			AutoUpdateReference();
		}

		private void AutoUpdateReference()
		{
			if (_sceneAsset == null)
			{
				if (string.IsNullOrEmpty(_scenePath))
					return;

				SceneAsset foundAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(_scenePath);
				if (foundAsset)
				{
					_sceneAsset = foundAsset;
					_isDirty = true;

					if (!Application.isPlaying)
					{
						// NOTE: This doesn't work for scriptable objects, hence the _isDirty.
						EditorSceneManager.MarkAllScenesDirty();
					}
				}
			}
			else
			{
				string foundPath = AssetDatabase.GetAssetPath(_sceneAsset);
				if (string.IsNullOrEmpty(foundPath))
					return;

				if (foundPath != _scenePath)
				{
					_scenePath = foundPath;
					_isDirty = true;

					if (!Application.isPlaying)
					{
						EditorSceneManager.MarkAllScenesDirty();
					}
				}
			}
		}
#endif
	}
}
