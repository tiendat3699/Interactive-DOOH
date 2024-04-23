using Cysharp.Threading.Tasks;
using GadGame.Manager;
using GadGame.MiniGame;
using UnityEngine;
using TMPro;

namespace GadGame.State.GameState
{
    public class EndGameState : State<MiniGameController>
    {
        public override async void Enter()
        {
            // Debug.Log("End Game");
            Runner.SetActive(false);
            await UniTask.Delay(1000);
            Runner.ShowResult();
            GameManager.Instance.CountDownEndGame();
            await UniTask.Delay(5000);
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