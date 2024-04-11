using UnityEngine;
using GadGame.Manager;

namespace GadGame.State
{
    public class RewardState : State<MainFlow>
    {
        private float _timer;
        
        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.RewardScene.ScenePath);   
            _timer = 0;
        }
        
        public override void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= 10)
            {
                Runner.SetState(typeof(CTAState));
            }
        }


        public override void Exit()
        {
            
        }
    }
}