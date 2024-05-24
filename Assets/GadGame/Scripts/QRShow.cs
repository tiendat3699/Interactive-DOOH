using System;
using Cysharp.Threading.Tasks;
using GadGame.Network;
using GadGame.Singleton;
using GraphQlClient.EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QRShow : Singleton<QRShow>
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private TMP_Text _mail;

    private bool _scanSuccess;
    public Action OnScanSuccess;

    void Update()
    {
        P4PGraphqlManager.Instance.OnGuestUpdatedSubscription += ShowInfo;
    }

    async void Start()
    {
       _rawImage.texture = await P4PGraphqlManager.Instance.GetQrLink();
    }

    private void ShowInfo(Guest guest) {
        _mail.text = guest.email;
        OnScanSuccess?.Invoke();
    }
}
