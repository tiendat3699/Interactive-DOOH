using GadGame.Manager;
using GadGame.MiniGame;
using UnityEngine;

namespace GadGame.State.GameState
{
    public class EndGameState : State<MiniGameController>
    {
        public override void Enter()
        {
            Debug.Log("End Game");
            GameManager.Instance.EndGame();
        }

        public override void Update(float time)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}