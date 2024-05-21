using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GadGame.Singleton;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GadGame.State.MainFlowState 
{
    public class PassByAnimation : Singleton<PassByAnimation>
    {
        public Animator passBy;
        [SerializeField] private RectTransform _transform;
        // [SerializeField] private RectTransform _videoIdleTransform;
        [SerializeField] private Image CircleImg;
        [SerializeField] private TextMeshProUGUI txtProgress;
        [SerializeField] private VideoPlayer videoPlayer;

        [Button]
        public void Play(bool engage) {
            // videoPlayer.gameObject.SetActive(!passBy);
            _transform.DOAnchorPosX(engage ? -1000 : 0, 1);
        }

        public async void SetPlayVideo(bool value){
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
        
        public void ReadyCountDown(float progress){
            CircleImg.fillAmount = progress ;
            txtProgress.text = Mathf.Floor(progress * 3).ToString();
        }
    }
}