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

        protected override void Awake()
        {
            base.Awake();
            _popup = Instantiate(_popupPrefab, transform, false);
            _popup.gameObject.SetActive(false);
        }
        
        public Popup Show(string message)
        {
            Show(message, 0).SetStay(true);
            return _popup;
        }

        public Popup Show(string message, float duration)
        {
            _popup.gameObject.SetActive(true);
            _popup.Show(message, duration * 1000);
            return _popup;
        }

        public void Hide()
        {
            _popup.gameObject.SetActive(true);
            _popup.Hide();
        }
    }
}