using Cysharp.Threading.Tasks;
using GadGame.Network;
using GraphQlClient.EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Test : MonoBehaviour
{

    [SerializeField] private RawImage _rawImage;
    [SerializeField] private TextMeshProUGUI _mail;

    void  OnEnable()
    {
        P4PGraphqlManager.Instance.OnGuestUpdatedSubscription += ShowInfo;
    }

    void OnDisable()
    {
        P4PGraphqlManager.Instance.OnGuestUpdatedSubscription -= ShowInfo;
    }

    async void Start()
    {
       await P4PGraphqlManager.Instance.LoginMachine();
       await P4PGraphqlManager.Instance.CreateGuest();
       await P4PGraphqlManager.Instance.JoinPromotion();
       await UniTask.Delay(1000);
       await P4PGraphqlManager.Instance.SubmitGameSession(1000);
       _rawImage.texture = await P4PGraphqlManager.Instance.GetQrLink();
    }

    private void ShowInfo(Guest guest) {
        Debug.Log(guest);
        _mail.text = guest.email;
    }
}