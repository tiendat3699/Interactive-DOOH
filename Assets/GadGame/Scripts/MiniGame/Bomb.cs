using System;
using GadGame.Manager;
using Pools.Runtime;
using UnityEngine;

namespace GadGame.MiniGame
{
    public class Bomb : MonoBehaviour, ICollect, IPoolable
    {
        [SerializeField] private int _reduceScore;
        [SerializeField] private Rigidbody2D _rb;
        
        private bool _inUsed;

        public void Init(float gravityScale = 1)
        {
            _rb.gravityScale = gravityScale;
        }

        private void FixedUpdate()
        {
            if (_inUsed && _rb.position.y <= -10)
            {
                this.Release();
            }
        }

        public void Collect()
        {
            GameManager.Instance.UpdateScore(-_reduceScore);
            this.Release();
        }

        public void OnGet()
        {
            _inUsed = true;
        }

        public void OnRelease()
        {
            _inUsed = false;
        }
    }
}