using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace GadGame.SO
{
    [CreateAssetMenu]
    public class SceneFlowConfig : Config
    {
        public SceneReference IdleScene;
        public SceneReference PassByScene;
        public SceneReference ViewedScene;
        public SceneReference EndGageScene;
        public SceneReference GameScene;
        public SceneReference CTASceneMale;
        public SceneReference CTASceneFemale;
        public SceneReference TMale;
    }
}