using GadGame.Manager;
using GadGame.MiniGame;
using UnityEngine;

namespace GadGame.State.GameState
{
    public class ResumeGameState : State<MiniGameController>
    {
        public override void Enter()
        {
            Debug.Log("Resume Game");
            Time.timeScale = 1;
        }

        public override void Update(float time)
        {
            Runner.SetState<PlayingGameState>();
        }

        public override void Exit()
        {
            
        }
    }
}