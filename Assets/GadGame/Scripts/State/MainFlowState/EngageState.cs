using GadGame.Manager;
using GadGame.Network;
using UnityEngine;

namespace GadGame.State
{
    public class EngageState : State<MainFlow>
    {
        private float _timer;

        public override void Enter()
        {
            LoadSceneManager.Instance.LoadSceneWithTransition(Runner.SceneFlowConfig.EndGageScene.ScenePath);
            _timer = 0;
        }

        public override void Update()
        {
            if (!Receiver.Instance.DataReceived.PassBy)
            {
                Runner.SetState(typeof(IdleState));
                return;
            }
            if (!Receiver.Instance.DataReceived.Engage)
            {
                Runner.SetState(typeof(ViewedState));
                return;
            }
            if (!Receiver.Instance.DataReceived.Ready) _timer = 0;
            _timer += Time.deltaTime;
            if (_timer >= 5)
            {
                Runner.SetState(typeof(PlayGameState));
            }
            
            Debug.Log(_timer);
        }

        public override void Exit()
        {
            
        }
    }
}