using TMPro;
using System;
using GadGame.MiniGame;
using UnityEngine;

namespace GadGame.Manager
{
    
    public class GameManager : Singleton.Singleton<GameManager>
    {
        private int _score;
        public int Score => _score;
        public event Action OnEnd;
        public event Action OnPause;
        public event Action OnResume;
        public event Action<int> OnScoreUpdate;

        public UnityEngine.UI.Image CircleImgEndGame;
        public TextMeshProUGUI txtProgressEndGame;

        private float left_time = 5f;
        private float countdown_time;

        public void UpdateScore(int value)
        {
            _score += value;
            if (_score <= 0) _score = 0;
            OnScoreUpdate?.Invoke(_score);
        }

        public void CountDownEndGame(){
            while(left_time >= 0){
                countdown_time = left_time / 5;
                CircleImgEndGame.fillAmount = countdown_time;
                txtProgressEndGame.text = Mathf.Floor(countdown_time * 5).ToString();
                left_time -= Time.deltaTime;
            }
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