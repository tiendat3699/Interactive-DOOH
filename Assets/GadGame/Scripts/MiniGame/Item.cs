using GadGame.Manager;
using Pools.Runtime;
using UnityEngine;

namespace GadGame.MiniGame
{
    public class Item : MonoBehaviour, ICollect
    {
        [SerializeField] private int _score;
        [SerializeField] private Rigidbody2D _rb;
        
        public void Init(float gravityScale = 1)
        {
            _rb.gravityScale = gravityScale;
        }
        
        private void LateUpdate()
        {
            if (_rb.position.y <= -10)
            {
                this.Release();
            }
        }
        
        public void Collect()
        {
            GameManager.Instance.UpdateScore(_score);
            this.Release();
        }
    }
}