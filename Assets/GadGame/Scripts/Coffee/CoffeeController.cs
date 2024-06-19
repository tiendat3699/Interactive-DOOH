using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GadGame.Event.Type;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GadGame.Scripts.Coffee
{
    public class CoffeeController : MonoBehaviour
    {
        [SerializeField] private VoidEvent _engageReady;
        [SerializeField] private BoolEvent _playPassByAnimEvent;
        [SerializeField] private BoolEvent _playVideoEvent;
        [SerializeField] private FloatEvent _readyCountDownEvent;
        [SerializeField] private CanvasGroup _idleBg;
        [SerializeField] private RawImage _userImager;
        [SerializeField] private Image _process;
        [SerializeField] private TextMeshProUGUI _hintText;
        [SerializeField] private Image _loading;
        [SerializeField] private string[] _texts;
        [SerializeField] private string[] _loadingTexts;

        private int _indexText;
        private bool _isLoading;
        private float _timer;

        private void Awake()
        {
            _idleBg.alpha = 1;
        }

        private void OnEnable()
        {
            _engageReady.Register(OnEngageReady);
            _playPassByAnimEvent.Register(Play);
            _playVideoEvent.Register(SetPlayVideo);
            _readyCountDownEvent.Register(SetReadyCountDown);
        }

        private void OnDisable()
        {
            _engageReady.Unregister(OnEngageReady);
            _playPassByAnimEvent.Unregister(Play);
            _playVideoEvent.Unregister(SetPlayVideo);
            _readyCountDownEvent.Unregister(SetReadyCountDown);
        }

        private void Update()
        {
            if(!_isLoading) return;
            _timer += Time.deltaTime;
            if (_timer >= 3)
            {
                _timer = 0;
                _indexText++;
                if (_indexText > _loadingTexts.Length - 1)
                {
                    _indexText = 0;
                }
                _hintText.text = _loadingTexts[_indexText];
            }
        }

        private void Play(bool engage) {
            // videoPlayer.gameObject.SetActive(!passBy);
            // _transform.DOAnchorPosX(engage ? -1000 : 0, 1);
            _hintText.text = _texts[0];
        }

        private void OnReceivedImage()
        {
            _loading.DOFade(0, 0.5f);
            _hintText.text = "Mô tả";
        }

        private void OnEngageReady()
        {
            _loading.gameObject.SetActive(false);
            _process.fillAmount = 0;
            _isLoading = true;
            _hintText.text = _loadingTexts[_indexText];
            _loading.DOFade(1, 1f);
            _loading.transform.DOLocalRotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetRelative(true)
                .SetEase(Ease.Linear);
        }

        private async void SetPlayVideo(bool value){
            if(value) {
                while (_idleBg.alpha < 1)
                {
                    _idleBg.alpha += Time.deltaTime * 3;
                    await UniTask.Yield();
                }
                _idleBg.alpha = 1;
            } else {
                while (_idleBg.alpha > 0) 
                {
                    _idleBg.alpha -= Time.deltaTime * 3;
                    await UniTask.Yield();
                }
                _idleBg.alpha = 0;
            }
        }
        
        private void SetReadyCountDown(float progress){
            _hintText.text = _texts[1];
            _process.fillAmount = 1- progress ;
        }
        
    }
}