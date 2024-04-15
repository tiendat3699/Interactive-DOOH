using GadGame.Manager;

namespace GadGame.State.MainFlowState
{
    public class RewardState : State<MainFlow>
    {

        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.RewardScene.ScenePath);
        }
        
        public override void Update(float time)
        {
            if (time >= 5)
            {
                Runner.SetState<CTAState>();
            }
        }


        public override void Exit()
        {
            
        }
    }
}