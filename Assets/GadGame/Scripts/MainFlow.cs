using System;
using GadGame.Manager;
using GadGame.SO;
using GadGame.State;
using GadGame.State.MainFlowState;
using Sirenix.OdinInspector;

namespace GadGame
{
    public class MainFlow : SingletonStateRunner<MainFlow>
    {
        public SceneFlowConfig SceneFlowConfig;
        public event Action<float> OnReadyCountDown; 
        public event Action<bool> OnReady; 

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetState<IdleState>();
        }

        public void ReadyCountDown(float duration)
        {
            OnReadyCountDown?.Invoke(duration);
        }

        public void Ready(bool ready)
        {
            OnReady?.Invoke(ready);
        }
    }
}