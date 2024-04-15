using GadGame.MiniGame;
using UnityEngine;

namespace GadGame.State.GameState
{
    public class ResumeGameState : State<MiniGameController>
    {
        public override void Enter()
        {
            Debug.Log("Resume Game");
            Runner.SetState<PlayingGameState>();
        }

        public override void Update(float time)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}