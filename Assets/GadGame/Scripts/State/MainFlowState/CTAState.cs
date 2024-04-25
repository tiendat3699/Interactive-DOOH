using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State.MainFlowState
{
    public class CTAState : State<MainFlow>
    {
        public override void Enter()
        {
            if(UdpSocket.Instance.DataReceived.Gender <= 0.3f) {
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneMale.ScenePath);
            } else if (UdpSocket.Instance.DataReceived.Gender >= 0.7f){
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneFemale.ScenePath);
            } else {
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.CTASceneFemale.ScenePath);
            }
        }

        public override void Update(float time)
        {


            if (time >= 10)
            {
                LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.PassByScene.ScenePath);
                Runner.SetState<IdleState>();
            }
        }

        public override void Exit()
        {

        }
    }
}