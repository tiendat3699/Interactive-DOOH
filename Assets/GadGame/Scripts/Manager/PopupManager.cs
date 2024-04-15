using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GadGame.Singleton;
using UnityEngine;

namespace GadGame.Manager
{
    public class PopupManager : PersistentSingleton<PopupManager>
    {
        [SerializeField] private Popup _popupPrefab;
        private Popup _popup;
        private CancellationTokenSource _cancellationTokenSource;

        protected override void Awake()
        {
            base.Awake();
            _popup = Instantiate(_popupPrefab, transform, false);
            _popup.gameObject.SetActive(false);
        }

        public async void Show(string message, int duration, Action onDone = null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _popup.gameObject.SetActive(true);
            _popup.Show(message);
            if (duration == -1) return;
            await UniTask.Delay(duration * 1000, cancellationToken: _cancellationTokenSource.Token);
            _popup.Hide();
            onDone?.Invoke();
        }

        public void Hide()
        {
            _popup.gameObject.SetActive(true);
            _cancellationTokenSource.Cancel();
            _popup.Hide();
        }
    }
}