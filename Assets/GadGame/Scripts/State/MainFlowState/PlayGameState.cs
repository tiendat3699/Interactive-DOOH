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
            _leaveTimer = 0; 
        }
        
        public override void Update(float time)
        {
            UdpSocket.Instance.SendDataToPython("1");
            // Debug.Log("Playing Game");
            switch (_warned)
            {
                case false when !UdpSocket.Instance.DataReceived.Engage:
                {
                    _leaveTimer += Time.deltaTime;
                    if ( _leaveTimer >= 5)
                    {
                        _warned = true;
                        _leaveTimer = 0;
                        _gameManager.Pause();
                        PopupManager.Instance.Show("Where Are You?", 10).OnComplete(OnWaringComplete);
                    }
                    return;
                }
                case true when UdpSocket.Instance.DataReceived.Engage:
                    _warned = false;
                    _gameManager.Resume();
                    PopupManager.Instance.Hide();
                    return;
            }
        }

        public override void Exit()
        {
            _warned = false;
            _gameManager.OnEnd -= OnEndGame;
        }

        private void OnEndGame()
        {
            Runner.SetState<CTAState>();
        }

        private void OnWaringComplete()
        {
            _gameManager.Resume();
            if(!UdpSocket.Instance.DataReceived.PassBy)
            {
                Runner.SetState<PassByState>();
                return;
            }

            if(!UdpSocket.Instance.DataReceived.Engage)
            {
                Runner.SetState<ViewedState>();
                return;
            }
            // Runner.SetState<EngageState>();
        }
    }
}