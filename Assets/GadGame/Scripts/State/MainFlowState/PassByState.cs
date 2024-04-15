using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State.MainFlowState
{
    public class PassByState : State<MainFlow>
    {

        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.PassByScene.ScenePath);
        }
        
        public override void Update(float time)
        {
            if (time < 2f) return;
            if (!DataReceiver.Instance.DataReceived.PassBy)
            {
                Runner.SetState<IdleState>();
                return;
            }
            if (DataReceiver.Instance.DataReceived.OnVision)
            {
                Runner.SetState<ViewedState>();
            }
        }

        public override void Exit()
        {
            
        }
    }
}