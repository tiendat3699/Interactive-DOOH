using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class CTAState : State<MainFlow>
    {

        private bool _scanSuccess;

        private float _leaveTimer;

        public override async void Enter()
        {
            if(UdpSocket.Instance.DataReceived.Gender < 0.5f) 
            {
                await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.CTASceneMale.ScenePath);
            } else 
            {
                await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.CTASceneFemale.ScenePath);
            }

            QRShow.Instance.OnScanSuccess += OnScanSuccess;

            _leaveTimer = 0;
        }

        public override void Update(float time)
        {
            if(_scanSuccess) return;
            if (!UdpSocket.Instance.DataReceived.Engage)
            {
                _leaveTimer += Time.deltaTime;
                if ( _leaveTimer >= 10)
                {
                    Runner.SetState<IdleState>();
                }
            } else {
                _leaveTimer = 0;
            }
            if (time >= 60)
            {
                Runner.SetState<IdleState>();
            }
        }

        public override void Exit()
        {
            QRShow.Instance.OnScanSuccess -= OnScanSuccess;
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.PassByScene.ScenePath);
        }

        private async void OnScanSuccess() {
            _scanSuccess = true;
            await UniTask.Delay(TimeSpan.FromSeconds(10));
            Runner.SetState<IdleState>();
        }

        private void LeaveComplete()
        {
            Runner.SetState<IdleState>();
        }
    }
}