using System.Diagnostics;
using GadGame.Manager;
using GadGame.Network;
using UnityEngine.SceneManagement;

namespace GadGame.State.MainFlowState
{
    public class IdleState : State<MainFlow>
    {
        public override void Enter()
        {
            PassByAnimation.Instance.Play(false);
            PassByAnimation.Instance.SetPlayVideo(true);
        }
        
        public override void Update(float time)
        {
            if(time < 2) return;

            if (UdpSocket.Instance.DataReceived.PassBy)
            {
                Runner.SetState<PassByState>();
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
            PassByAnimation.Instance.SetPlayVideo(false);
        }
    }
}