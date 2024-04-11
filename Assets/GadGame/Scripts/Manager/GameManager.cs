
using System;
using Sirenix.OdinInspector;

namespace GadGame.Manager
{
    public class GameManager : Singleton.Singleton<GameManager>
    {
        public event Action OnEnd;
        
        [Button]
        private void EndGame()
        {
            OnEnd?.Invoke();
        }
    }
}