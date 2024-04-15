using UnityEngine;
using GadGame.MiniGame;

namespace GadGame.State.GameState
{
    public class PlayingGameState : State<MiniGameController>
    {
        private float _playingTime;
        
        public override void Enter()
        {
            Debug.Log("Playing Game");
        }

        public override void Update(float time)
        {
            if(time < 1) return;
            _playingTime += Time.deltaTime;
            var remainingTime = Mathf.Clamp(Runner.GameTime - _playingTime, 0, Runner.GameTime);
            Runner.SetTextTime(remainingTime);
            Runner.PlayerControl();
            Runner.SpawnRandomItem();
            if (_playingTime >= Runner.GameTime)
            {
                Runner.SetState<EndGameState>();
            }
        }

        public override void Exit()
        {
            
        }
    }
}