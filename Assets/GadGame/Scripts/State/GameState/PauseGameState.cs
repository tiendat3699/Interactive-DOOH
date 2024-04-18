using GadGame.MiniGame;
using UnityEngine;

namespace GadGame.State.GameState
{
    public class PauseGameState : State<MiniGameController>
    {
        public override void Enter()
        {
            Debug.Log("Pause Game");
            Time.timeScale = 0;
        }

        public override void Update(float time)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}