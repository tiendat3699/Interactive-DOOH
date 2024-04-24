using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class CTAState : State<MainFlow>
    {
        public override void Enter()
        {
            if(UdpSocket.Instance.DataReceived.Gender <= 0.5f) {
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneMale.ScenePath);
            } else {
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneFemale.ScenePath);
            }
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

                if(!UdpSocket.Instance.DataReceived.Engage)
                {
                    Runner.SetState<ViewedState>();
                    return;
                }
                Runner.SetState<PassByState>();
            }
        }

        public override void Exit()
        {

        }
    }
}