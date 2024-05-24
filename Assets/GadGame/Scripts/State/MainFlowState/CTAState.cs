using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State.MainFlowState
{
    public class CTAState : State<MainFlow>
    {

        private bool _scanSuccess;

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
        }

        public async override void Update(float time)
        {
            if(_scanSuccess) return;
            if (time >= 60)
            {
                Runner.SetState<IdleState>();
                await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.IdleScene.ScenePath);
            }
        }

        public override void Exit()
        {
            QRShow.Instance.OnScanSuccess -= OnScanSuccess;
        }

        private async void OnScanSuccess() {
            _scanSuccess = true;
            await UniTask.Delay(TimeSpan.FromSeconds(10));
            await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(Runner.SceneFlowConfig.IdleScene.ScenePath);
            Runner.SetState<IdleState>();
        }
    }
}