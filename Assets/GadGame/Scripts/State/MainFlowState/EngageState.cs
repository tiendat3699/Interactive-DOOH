using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State.MainFlowState
{
    public class EngageState : State<MainFlow>
    {
        private float _readyTimer;
        private bool _warned;

        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.EndGageScene.ScenePath);
            _readyTimer = 0;
        }

        public override void Update(float time)
        {
            if (time >= 2)
            {
                if (!DataReceiver.Instance.DataReceived.PassBy)
                {
                    Runner.SetState<IdleState>();
                    return;
                }
                
                switch (_warned)
                {
                    case false when !DataReceiver.Instance.DataReceived.Engage:
                        _warned = true;
                        PopupManager.Instance.Show("Come Back", 5, () =>
                        {
                            Runner.SetState<ViewedState>();
                        });
                        return;
                    case true when DataReceiver.Instance.DataReceived.Engage:
                        _warned = false;
                        PopupManager.Instance.Hide();
                        return;
                }
            }
            if (!DataReceiver.Instance.DataReceived.Ready) _readyTimer = 0;
            _readyTimer += Time.deltaTime;
            if (_readyTimer >= 5)
            {
                Runner.SetState<PlayGameState>();
            }
        }

        public override void Exit()
        {
            _warned = false;
        }
    }
}