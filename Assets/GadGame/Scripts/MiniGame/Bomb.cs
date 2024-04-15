using GadGame.Manager;
using Pools.Runtime;
using UnityEngine;

namespace GadGame.MiniGame
{
    public class Bomb : MonoBehaviour, ICollect
    {
        [SerializeField] private int _reduceScore;
        
        public void Collect()
        {
            GameManager.Instance.UpdateScore(-_reduceScore);
            this.Release();
        }
    }
}