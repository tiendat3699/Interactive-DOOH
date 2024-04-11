using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State
{
    public class PassByState : State<MainFlow>
    {

        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.PassByScene.ScenePath);
        }
        
        public override void Update()
        {
            if (!Receiver.Instance.DataReceived.PassBy)
            {
                Runner.SetState(typeof(IdleState));
                return;
            }
            if (Receiver.Instance.DataReceived.OnVision)
            {
                Runner.SetState(typeof(ViewedState));
            }
        }

        public override void Exit()
        {
            
        }
    }
}