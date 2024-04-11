using GadGame.Manager;

namespace GadGame.State
{
    public class PlayGameState : State<MainFlow>
    {
        private GameManager _gameManager;
        
        public override async void Enter()
        {
            await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.GameScene.ScenePath);
            _gameManager = GameManager.Instance;
            _gameManager.OnEnd += OnEndGame;
        }
        
        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }

        private void OnEndGame()
        {
            Runner.SetState(typeof(RewardState));
        }
    }
}