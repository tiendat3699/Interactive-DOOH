using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class PlayGameState : State<MainFlow>
    {
        private GameManager _gameManager;
        private float _leaveTimer;
        private bool _warned;
        
        public override async void Enter()
        {
            await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.GameScene.ScenePath);
            _gameManager = GameManager.Instance;
            _gameManager.OnEnd += OnEndGame;
        }
        
        public override void Update(float time)
        {
            switch (_warned)
            {
                case false when !DataReceiver.Instance.DataReceived.Engage:
                {
                    _leaveTimer += Time.deltaTime;
                    if ( _leaveTimer >= 5)
                    {
                        _warned = true;
                        _leaveTimer = 0;
                        _gameManager.Pause();
                        PopupManager.Instance.Show("Where Are You?", 30, () => Runner.SetState<IdleState>());
                    }
                    return;
                }
                case true when DataReceiver.Instance.DataReceived.Engage:
                    _warned = false;
                    _gameManager.Resume();
                    PopupManager.Instance.Hide();
                    return;
            }
        }

        public override void Exit()
        {
            
        }

        private void OnEndGame()
        {
            Runner.SetState<RewardState>();
        }
    }
}