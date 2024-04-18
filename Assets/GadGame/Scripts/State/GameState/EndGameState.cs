using Cysharp.Threading.Tasks;
using GadGame.Manager;
using GadGame.MiniGame;
using UnityEngine;

namespace GadGame.State.GameState
{
    public class EndGameState : State<MiniGameController>
    {
        public override async void Enter()
        {
            Debug.Log("End Game");
            Runner.DisableBasket();
            await UniTask.Delay(1000);
            Runner.ShowResult();
            await UniTask.Delay(4000);
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