using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class EngageState : State<MainFlow>
    {
        private float _readyTimer;
        private bool _warned;
        private bool _showCountDown;

        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.EndGageScene.ScenePath);
            _readyTimer = 5;
        }

        public override void Update(float time)
        {
            if (time >= 2)
            {
                switch (_warned)
                {
                    case false when !UdpSocket.Instance.DataReceived.PassBy:
                        Runner.SetState<IdleState>();
                        break;
                    case false when !UdpSocket.Instance.DataReceived.Engage:
                        _warned = true;
                        PopupManager.Instance.Show("Come Back", 5).OnComplete(OnWaringComplete);
                        break;
                    case true when UdpSocket.Instance.DataReceived.Engage:
                        _warned = false;
                        PopupManager.Instance.Hide();
                        break;
                }
                
                switch (_showCountDown)
                {
                    case false when UdpSocket.Instance.DataReceived.Ready:
                        _showCountDown = true;
                        Runner.Ready(true);
                        break;
                    case true when !UdpSocket.Instance.DataReceived.Ready:
                        _showCountDown = false;
                        Runner.Ready(false);
                        break;
                }

                if (!UdpSocket.Instance.DataReceived.Ready) _readyTimer = 5;
                _readyTimer -= Time.deltaTime;
                if (_readyTimer <= 0)
                {
                    _readyTimer = 0;
                    Runner.SetState<PlayGameState>();
                }
                Runner.ReadyCountDown(_readyTimer);
            }
            
        }

        public override void Exit()
        {
            _warned = false;
        }

        private void OnWaringComplete()
        {
            if (!UdpSocket.Instance.DataReceived.PassBy)
            {
                Runner.SetState<IdleState>();
                return;
            }

            if (!UdpSocket.Instance.DataReceived.OnVision)
            {
                Runner.SetState<PassByState>();
                return;
            }

            Runner.SetState<ViewedState>();
        }
    }
}