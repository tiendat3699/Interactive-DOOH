using DG.Tweening;
using GadGame.Manager;
using Pools.Runtime;
using UnityEngine;

namespace GadGame.MiniGame
{
    public class Bomb : MonoBehaviour, ICollect, IPoolable
    {
        [SerializeField] private int _reduceScore;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TrailRenderer _trail;
        
        private bool _inUsed;
        
        private Tweener _tweener;

        public void Init(float gravityScale = 1)
        {
            _rb.gravityScale = gravityScale;
            _tweener = _sprite.transform.DORotate(new Vector3(0, 0, gravityScale * 50),0.1f).SetLoops(-1, LoopType.Incremental);
        }

        private void LateUpdate()
        {
            if (_inUsed && _rb.position.y <= -10)
            {
                this.Release();
            }
        }

        public void Collect()
        {
            GameManager.Instance.UpdateScore(-_reduceScore);
            SoundManager.Instance.PlaySfx(SoundDefine.GetHit);
            this.Release();
        }

        public void OnGet()
        {
            _inUsed = true;
        }

        public void OnRelease()
        {
            _trail.Clear();
            _inUsed = false;
            _tweener.Restart();
            _tweener.Complete();
            _tweener.Kill();
        }
    }
}