using UnityEngine;

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
        public SceneReference RewardScene;
        public SceneReference CTAScene;
    }
}