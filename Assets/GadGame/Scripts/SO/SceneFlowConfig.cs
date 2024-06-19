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
        public SceneReference EndGageScene;
        public SceneReference GameMaleScene;
        public SceneReference GameFemaleScene;
        public SceneReference CTASceneMale;
        public SceneReference CTASceneFemale;
    }
}