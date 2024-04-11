using GadGame.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GadGame
{
    public class TransitionSceneDemo : MonoBehaviour
    {
        [SerializeField] private string _scenePath;
        
        [Button, HideInEditorMode]
        public void LoadScene()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(_scenePath, 2);
        }
    }
}