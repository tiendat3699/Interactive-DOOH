using GadGame.Manager;
using Pools.Runtime;
using UnityEngine;

namespace GadGame.MiniGame
{
    public class Item : MonoBehaviour, ICollect
    {
        [SerializeField] private int _score;
        
        public void Collect()
        {
            GameManager.Instance.UpdateScore(_score);
            this.Release();
        }
    }
}