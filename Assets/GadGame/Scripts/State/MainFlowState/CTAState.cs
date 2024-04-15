using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class CTAState : State<MainFlow>
    {
        private float _noPassByTimer;
        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTAScene.ScenePath);
        }

        public override void Update(float time)
        {
            if (!DataReceiver.Instance.DataReceived.PassBy)
            {
                _noPassByTimer += Time.deltaTime;
                if (_noPassByTimer >= 10)
                {
                    Runner.SetState<IdleState>();
                }
            }
            else
            {
                _noPassByTimer = 0;
            }
        }

        public override void Exit()
        {
            _noPassByTimer = 0;
        }
    }
}