using System;
using GadGame.Manager;
using GadGame.SO;
using GadGame.State;
using GadGame.State.MainFlowState;
using Sirenix.OdinInspector;
using GadGame.Network;


namespace GadGame
{
    public class MainFlow : SingletonStateRunner<MainFlow>
    {
        public SceneFlowConfig SceneFlowConfig;
        public event Action<float> OnReadyCountDown; 
        public event Action<bool> OnReady; 

        protected override async void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            await P4PGraphqlManager.Instance.LoginMachine();
        }

        private  async void Start()
        {
            await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(SceneFlowConfig.PassByScene.ScenePath);
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