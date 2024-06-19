using GadGame.Manager;
using GadGame.Network;
using Unity.Mathematics;
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
            // LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.EndGageScene.ScenePath);
            Runner.PlayPassByAnim.Raise(true);
            _readyTimer = 5;
        }

        public override void Update(float time)
        {
            if(!UdpSocket.Instance.DataReceived.PassBy) {
                Runner.SetState<IdleState>();
                return;
            }

            if(!UdpSocket.Instance.DataReceived.Engage) {
                Runner.SetState<PassByState>();
                return;
            }

            if (!UdpSocket.Instance.DataReceived.Ready) _readyTimer = 3;
            Runner.ReadyCountDown.Raise(_readyTimer / 3);
            _readyTimer -= Time.deltaTime;
            if (_readyTimer <= 0)
            {
                _readyTimer = 0;
                Runner.EngageReady.Raise();
                // Runner.SetState<PlayGameState>();
            }
            if (time >= 2)
            {

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