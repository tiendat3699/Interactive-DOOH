using DG.Tweening;
using GadGame.Network;
using GadGame.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GadGame.State.MainFlowState 
{
    public class PassByAnimation : Singleton<PassByAnimation>
    {
        public Animator passBy;
        [SerializeField] private RectTransform _transform;


        [Button]
        public void Play(bool engage) {
            _transform.DOAnchorPosX(engage ?  -1000 : 0, 2);
        }
    }
}