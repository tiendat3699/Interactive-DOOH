using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class CTAState : State<MainFlow>
    {
        public override void Enter()
        {
            if(UdpSocket.Instance.DataReceived.Gender <= 0.3f) {
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneMale.ScenePath);
            } else if (UdpSocket.Instance.DataReceived.Gender >= 0.7f){
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneFemale.ScenePath);
            } else {
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneBoth.ScenePath);
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