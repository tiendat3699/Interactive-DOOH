using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State
{
    public class ViewedState : State<MainFlow>
    {
        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.ViewedScene.ScenePath);
        }
        
        public override void Update()
        {
            if (Receiver.Instance.DataReceived.Engage)
            {
                Runner.SetState(typeof(EngageState));
            }
        }

        public override void Exit()
        {
            
        }
    }
}