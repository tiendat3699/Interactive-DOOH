using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GadGame.Manager
{

    public class GameManager : Singleton.Singleton<GameManager>
    {
        public bool Playing {get; private set;}
        private int _score;
        public int Score => _score;
        public event Action OnEnd;
        public event Action OnPause;
        public event Action OnResume;
        public event Action<int> OnScoreUpdate;

        public void UpdateScore(int value)
        {
            _score += value;
            if (_score <= 0) _score = 0;
            OnScoreUpdate?.Invoke(_score);
        }

        public void StartPlay()
        {
            Playing = true;
        }

        public void EndPlay()
        {
            Playing = false;
        }
        public void EndGame()
        {
            OnEnd?.Invoke();
        }

        public void Pause()
        {
            OnPause?.Invoke();
        }

        public void Resume()
        {
            OnResume?.Invoke();
        }
    }
}