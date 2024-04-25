using GadGame.Manager;
using GadGame.Network;
using UnityEngine;


namespace GadGame.State.MainFlowState
{
    public class PassByState : State<MainFlow>
    {
        public async override void Enter()
        {
            // await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.PassByScene.ScenePath);
            PassByAnimation.Instance.Play(false);
        }
        
        public override void Update(float time)
        {
            // if(time < 2) return;
            if (!UdpSocket.Instance.DataReceived.PassBy)
            {
                Runner.SetState<IdleState>();
                return;
            }

            if (UdpSocket.Instance.DataReceived.Engage)
            {
                Runner.SetState<EngageState>();

                return;
            }
        }

        public override void Exit()
        {
            
        }
    }
}