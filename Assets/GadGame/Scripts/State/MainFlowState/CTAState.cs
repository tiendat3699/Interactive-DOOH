using UnityEngine;
using GadGame.Manager;

namespace GadGame.State
{
    public class CTAState : State<MainFlow>
    {
        private float _timer;

        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTAScene.ScenePath);
            _timer = 0;
        }

        public override void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= 10)
            {
                Runner.SetState(typeof(IdleState));
            } 
        }

        public override void Exit()
        {
            
        }
    }
}