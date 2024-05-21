using GadGame.Manager;
using GadGame.MiniGame;

namespace GadGame.State.GameState
{
    public class TutorialGameState : State<MiniGameController>
    {
        private bool _turnedRight;
        private bool _turnedLeft;
        
        public override void Enter()
        {
            Runner.ShowTutorial();
        }

        public override void Update(float time)
        {
            if (time > 2)
            {
                Runner.PlayerControl();
            }

            if(time >= 60) {
                GameManager.Instance.EndGame();
            }
        }

        public override void Exit()
        {
            
        }
    }
}