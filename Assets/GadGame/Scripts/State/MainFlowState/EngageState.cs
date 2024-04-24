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

        private PassByAnimation passByAnim;

        public override void Enter()
        {
            passByAnim = PassByAnimation.Instance;
            passByAnim.Play(true);
            // LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.EndGageScene.ScenePath);
            _readyTimer = 5;
        }

        public override void Update(float time)
        {
            if (time >= 2)
            {
                switch (_warned)
                {
                    case true when !UdpSocket.Instance.DataReceived.PassBy:
                        Runner.SetState<IdleState>();
                        break;
                    case false when !UdpSocket.Instance.DataReceived.Engage:
                        _warned = true;
                        passByAnim.Play(false);
                        // PopupManager.Instance.Show("Come Back", 5).OnComplete(OnWaringComplete);
                        break;
                    case true when UdpSocket.Instance.DataReceived.Engage:
                        _warned = false;
                        passByAnim.Play(true);
                        // PopupManager.Instance.Hide();
                        break;
                }
                
                switch (_showCountDown)
                {
                    case false when UdpSocket.Instance.DataReceived.Ready:
                        _showCountDown = true;
                        // Runner.Ready(true);
                        break;
                    case true when !UdpSocket.Instance.DataReceived.Ready:
                        _showCountDown = false;
                        // Runner.Ready(false);
                        break;
                }

                if (!UdpSocket.Instance.DataReceived.Ready) _readyTimer = 3;
                passByAnim.ReadyCountDown(_readyTimer / 3);
                _readyTimer -= Time.deltaTime;
                if (_readyTimer <= 0)
                {
                    _readyTimer = 0;
                    Runner.SetState<PlayGameState>();
                }
                // Runner.ReadyCountDown(_readyTimer);
            }
            
        }

        public override void Exit()
        {
            _warned = false;
        }

        // private void OnWaringComplete()
        // {
        //     if (!UdpSocket.Instance.DataReceived.PassBy)
        //     {
        //         Runner.SetState<IdleState>();
        //         return;
        //     }

        //     if (!UdpSocket.Instance.DataReceived.OnVision)
        //     {
        //         Runner.SetState<PassByState>();
        //         return;
        //     }

        //     Runner.SetState<ViewedState>();
        // }
    }
}