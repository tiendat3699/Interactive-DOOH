﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GadGame.Event.Type;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GadGame
{
    public class ReadyPopUp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Transform _content;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private FloatEvent ReadyCountDown;

        private float _readyTime;
        
        private void Start()
        {
            ReadyCountDown.Register(OnReadyCountdown);
            // _mainFlow.OnReady += OnReady;
            _content.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            _canvasGroup.DOKill();
            _content.DOKill();
            ReadyCountDown.Unregister(OnReadyCountdown);
            // _mainFlow.OnReady -= OnReady;
        }

        private async void OnReady(bool ready)
        {
            if (ready)
            {
                await _canvasGroup.DOFade(1, 0.1f);
                await _content.DOScale(Vector3.one, 0.2f);
            }
            else
            {
                await _content.DOScale(Vector3.zero, 0.2f);
                await _canvasGroup.DOFade(0, 0.1f);
            }
        }

        private void OnReadyCountdown(float duration)
        {
            _time.text = Mathf.CeilToInt(duration).ToString();
            _readyTime = 5 - duration;
            _fillImage.fillAmount = _readyTime / 5;  
        }
        
    }
}