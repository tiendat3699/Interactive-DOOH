using System;
using Cysharp.Threading.Tasks;
using Pools.Runtime;
using UnityEngine;

namespace GadGame
{
    public class AudioObject : MonoBehaviour, IPoolable
    {
        [SerializeField] private AudioSource _source;
        private bool _alive;

        private void Update()
        {
            if (_alive && !_source.isPlaying)
            {
                this.Release();
            }
        }

        public void Play(AudioClip audioClip, float volume = 1, bool loop = false)
        {
            _source.clip = audioClip;
            _source.volume = 1;
            _source.loop = loop;
            _source.Play();
            _alive = true;
        }

        public void Stop()
        {
            _source.Stop();
        }

        public void OnGet()
        {
            _source.Stop();
            _source.clip = null;
            _source.loop = false;
        }

        public void OnRelease()
        {
            _alive = false;
            _source.clip = null;
            _source.loop = false;
        }
    }
}