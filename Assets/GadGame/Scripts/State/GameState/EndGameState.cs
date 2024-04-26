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
            Runner.SetActive(false);
            await UniTask.Delay(1000);
            Runner.ShowResult();
        }

        public override void Update(float time)
        {
            Runner.CountDownEndGame(time);
            if(time >= 10) {
                GameManager.Instance.EndGame();
            }
        }

        public override void Exit()
        {
            
        }
    }
}