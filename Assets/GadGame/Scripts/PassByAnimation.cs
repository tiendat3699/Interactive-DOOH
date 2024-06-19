using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GadGame.Event.Type;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GadGame.State.MainFlowState 
{
    public class PassByAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _passBy;
        [SerializeField] private RectTransform _transform;
        // [SerializeField] private RectTransform _videoIdleTransform;
        [SerializeField] private Image CircleImg;
        [SerializeField] private TextMeshProUGUI txtProgress;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private BoolEvent _playPassByAnimEvent;
        [SerializeField] private BoolEvent _playVideoEvent;
        [SerializeField] private FloatEvent _readyCountDownEvent;

        private void OnEnable()
        {
            _playPassByAnimEvent.Register(Play);
            _playVideoEvent.Register(SetPlayVideo);
            _readyCountDownEvent.Register(SetReadyCountDown);
        }

        private void OnDisable()
        {
            _playPassByAnimEvent.Unregister(Play);
            _playVideoEvent.Unregister(SetPlayVideo);
            _readyCountDownEvent.Unregister(SetReadyCountDown);
        }

        private void Play(bool engage) {
            // videoPlayer.gameObject.SetActive(!passBy);
            _transform.DOAnchorPosX(engage ? -1000 : 0, 1);
        }

        private async void SetPlayVideo(bool value){
            if(value) {
                while (videoPlayer.targetCameraAlpha < 1) 
                {
                    videoPlayer.targetCameraAlpha += Time.deltaTime * 3;
                    await UniTask.Yield();
                }
                
                videoPlayer.targetCameraAlpha = 1;
            } else {
                while (videoPlayer.targetCameraAlpha > 0) 
                {
                    videoPlayer.targetCameraAlpha -= Time.deltaTime * 3;
                    await UniTask.Yield();
                }

                videoPlayer.targetCameraAlpha = 0;
            }
        }
        
        private void SetReadyCountDown(float progress){
            CircleImg.fillAmount = progress ;
            txtProgress.text = Mathf.Floor(progress * 3).ToString();
        }
    }
}