using Cysharp.Threading.Tasks;
using GadGame.Network;

namespace GadGame.State.MainFlowState
{
    public class IdleState : State<MainFlow>
    {
        public override async void Enter()
        {
            await UniTask.Delay(1000);
            Runner.PlayPassByAnim.Raise(false);
            Runner.PlayVideo.Raise(true);
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
        }

        public override void Exit()
        {
            Runner.PlayVideo.Raise(false);
        }
    }
}