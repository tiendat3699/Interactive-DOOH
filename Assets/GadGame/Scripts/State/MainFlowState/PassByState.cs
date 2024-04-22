using GadGame.Manager;
using GadGame.Network;
using UnityEngine;


namespace GadGame.State.MainFlowState
{
    public class PassByState : State<MainFlow>
    {
        private PassByAnimation passByAnim;
        public async override void Enter()
        {
            await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.PassByScene.ScenePath);
            passByAnim = PassByAnimation.Instance;
            passByAnim.Play(false);
        }
        
        public override void Update(float time)
        {
            if (time < 2f) return;
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
            
            // if (UdpSocket.Instance.DataReceived.OnVision)
            // {
            //     Runner.SetState<ViewedState>();
            //     return;
            // }
        }

        public override void Exit()
        {
            
        }
    }
}