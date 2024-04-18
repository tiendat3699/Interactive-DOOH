using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GadGame
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _content;
        
        private float _duration;
        private Action _onComplete;
        private Action<float> _onRun;
        private bool _active = false;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _stay;

        private async void PopupUpdate()
        {
            while (_active)
            {
                await UniTask.Delay(100, cancellationToken:_cancellationTokenSource.Token, ignoreTimeScale: true);
                if (!_stay)
                {
                    _duration -= 100;
                    if (_duration <= 0)
                    {
                        Hide();
                    }
                }
                _onRun?.Invoke(_duration);
            }
        }

        public void SetStay(bool value)
        {
            _stay = value;
        }

        public async void Show(string message, float duration)
        {
            _active = true;
            _duration = duration;
            _stay = false;
            _cancellationTokenSource = new CancellationTokenSource();
            _content.DOComplete();
            _canvasGroup.DOComplete();
            _message.text = message;
            _canvasGroup.alpha = 0;
            _content.localScale = Vector3.zero;
            await _canvasGroup.DOFade(1, 0.3f).SetUpdate(true);
            await _content.DOScale(Vector3.one, 0.5f).SetUpdate(true);
            PopupUpdate();
        }

        public async void Hide()
        {
            _active = false;
            _duration = 0;
            _cancellationTokenSource.Cancel();
            _content.DOComplete();
            _canvasGroup.DOComplete();
            await _content.DOScale(Vector3.zero, 0.3f).SetUpdate(true);
            await _canvasGroup.DOFade(0, 0.3f).SetUpdate(true);
            _onComplete?.Invoke();
            _onComplete = null;
            _onRun = null;
        }

        public void OnComplete(Action action)
        {
            _onComplete = action;
        }
        
        public void OnRun(Action<float> action)
        {
            _onRun = action;
        }
    }
}