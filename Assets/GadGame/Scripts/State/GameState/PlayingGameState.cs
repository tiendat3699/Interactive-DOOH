using UnityEngine;
using GadGame.MiniGame;
using GadGame.Manager;
using GadGame.Network;

namespace GadGame.State.GameState
{
    public class PlayingGameState : State<MiniGameController>
    {
        private float _playingTime;
        
        public override async void Enter()
        {
            await P4PGraphqlManager.Instance.CreateGuest();
            await P4PGraphqlManager.Instance.JoinPromotion();
            GameManager.Instance.StartPlay();
            Runner.SetActive(true);
        }

        public override void Update(float time)
        {
            Runner.PlayerControl();
            if(time < 1) return;
            _playingTime += Time.deltaTime;
            var remainingTime = Mathf.Clamp(Runner.GameTime - _playingTime, 0, Runner.GameTime);
            Runner.SetTextTime(remainingTime);
            Runner.SpawnRandomItem();
            if (_playingTime >= Runner.GameTime)
            {
                UdpSocket.Instance.SendDataToPython("Done");  
                Runner.SetState<EndGameState>();
            }
        }

        public override void Exit()
        {
            GameManager.Instance.EndPlay();
        }
    }
}