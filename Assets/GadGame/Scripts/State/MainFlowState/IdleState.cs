using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State.MainFlowState
{
    public class IdleState : State<MainFlow>
    {
        
        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.IdleScene.ScenePath);
        }
        
        public override void Update(float time)
        {
            if(time < 2) return;
            if (UdpSocket.Instance.DataReceived.PassBy)
            {
                Runner.SetState<PassByState>();
            }
        }

        public override void Exit()
        {
           
        }
    }
}