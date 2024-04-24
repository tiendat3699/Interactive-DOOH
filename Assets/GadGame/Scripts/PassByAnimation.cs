using DG.Tweening;
using GadGame.Network;
using GadGame.Singleton;
using Microsoft.Unity.VisualStudio.Editor;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GadGame.State.MainFlowState 
{
    public class PassByAnimation : Singleton<PassByAnimation>
    {
        public Animator passBy;
        [SerializeField] private RectTransform _transform;
        [SerializeField] private UnityEngine.UI.Image CircleImg;
        [SerializeField] private TextMeshProUGUI txtProgress;
        // [SerializeField] [Range(0,1)] float progress = 1f;


        [Button]
        public void Play(bool engage) {
            _transform.DOAnchorPosX(engage ?  -1000 : 0, 2);
        }

        public void ReadyCountDown(float progress){
            CircleImg.fillAmount = progress ;
            txtProgress.text = Mathf.Floor(progress * 3).ToString();
        }
    }
}