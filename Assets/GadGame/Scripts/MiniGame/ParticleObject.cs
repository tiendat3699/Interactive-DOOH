using System;
using Pools.Runtime;
using UnityEngine;

namespace GadGame.MiniGame
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleObject : MonoBehaviour, IPoolable
    {

        private ParticleSystem _particle;
        private bool _alive;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (_alive && _particle.isStopped)
            {
                this.Release();
            }
        }

        public void OnGet()
        {
            _alive = true;
        }

        public void OnRelease()
        {
            _alive = false;
        }
    }
}