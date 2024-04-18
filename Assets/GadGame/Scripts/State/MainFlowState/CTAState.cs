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
            if (!UdpSocket.Instance.DataReceived.PassBy)
            {
                _noPassByTimer += Time.deltaTime;
                if (_noPassByTimer >= 5)
                {
                    if(!UdpSocket.Instance.DataReceived.PassBy)
                    {
                        Runner.SetState<IdleState>();
                        return;
                    }
                    if(!UdpSocket.Instance.DataReceived.OnVision)
                    {
                        Runner.SetState<PassByState>();
                        return;
                    }
                    if(!UdpSocket.Instance.DataReceived.Engage)
                    {
                        Runner.SetState<ViewedState>();
                        return;
                    }
                    Runner.SetState<EngageState>();
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