using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State
{
    public class IdleState : State<MainFlow>
    {
        
        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.IdleScene.ScenePath);
        }
        
        public override void Update()
        {
            if (Receiver.Instance.DataReceived.PassBy)
            {
                Runner.SetState(typeof(PassByState));
            }
        }

        public override void Exit()
        {
           
        }
    }
}