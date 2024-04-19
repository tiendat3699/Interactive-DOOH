using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class CTAState : State<MainFlow>
    {
        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTAScene.ScenePath);
        }

        public override void Update(float time)
        {
            if (time >= 10)
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

        public override void Exit()
        {

        }
    }
}