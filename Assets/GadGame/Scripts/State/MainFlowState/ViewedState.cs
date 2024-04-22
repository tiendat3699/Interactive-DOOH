using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State.MainFlowState
{
    public class ViewedState : State<MainFlow>
    {
        public override void Enter()
        {
            // LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.ViewedScene.ScenePath);
        }
        
        public override void Update(float time)
        {
            if(time < 2) return; 
            if (!UdpSocket.Instance.DataReceived.PassBy)
            {
                Runner.SetState<IdleState>();
                return;
            }
            if (UdpSocket.Instance.DataReceived.Engage)
            {
                Runner.SetState<EngageState>();
            }
        }

        public override void Exit()
        {
            
        }
    }
}