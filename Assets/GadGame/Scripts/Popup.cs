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

        public async void Show(string message)
        {
            _content.DOComplete();
            _canvasGroup.DOComplete();
            _message.text = message;
            _canvasGroup.alpha = 0;
            _content.localScale = Vector3.zero;
            await _canvasGroup.DOFade(1, 0.3f);
            await _content.DOScale(Vector3.one, 0.5f);
        }

        public async void Hide()
        {
            _content.DOComplete();
            _canvasGroup.DOComplete();
            await _content.DOScale(Vector3.zero, 0.3f);
            await _canvasGroup.DOFade(0, 0.3f);
        }
    }
}