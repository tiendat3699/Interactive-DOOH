using GadGame.Singleton;
using GadGame.SO;
using Pools.Runtime;
using UnityEngine;

namespace GadGame.Manager
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        private Pool<AudioObject> _pool = new();
        private AudioConfig _audioConfig;
        private AudioSource _musicAudioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioConfig = ConfigHelper.GetConfig<AudioConfig>();
            _musicAudioSource = gameObject.AddComponent<AudioSource>();
            _musicAudioSource.playOnAwake = false;
            _pool = new Pool<AudioObject>(_audioConfig.AudioObjectPrefab, 10, 1000);
        }

        public AudioObject PlaySfx(SoundDefine soundDefine, float volume = 1, bool loop = false)
        {
            var audioSource = _pool.Get();
            audioSource.transform.SetParent(transform);
            var audioClip = _audioConfig.Sounds[soundDefine.ToString()];
            audioSource.Play(audioClip, volume, loop);
            return audioSource;
        }

        public void PlayMusic(MusicDefine musicDefine, float volume = 1, bool loop = true)
        {
            _musicAudioSource.Stop();
            var audioClip = _audioConfig.Musics[musicDefine.ToString()];
            _musicAudioSource.clip = audioClip;
            _musicAudioSource.volume = volume;
            _musicAudioSource.loop = loop;
            _musicAudioSource.Play();
        }

        public void StopMusic()
        {
            _musicAudioSource.Stop();
            _musicAudioSource.clip = null;
            _musicAudioSource.loop = false;
        }
    }
}