using System;
using GadGame.Event.Customs;
using GadGame.Event.Type;
using GadGame.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QRShow : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private TextMeshProUGUI _textShow;
    [SerializeField] private Animator _voucherAnimator;
    [SerializeField] private Animator _qrAnimator;
    [SerializeField] private Animator _notifyAnimator;
    [SerializeField] private Animator _warningAnimator;
    [SerializeField] private Timer _timer;
    [SerializeField] private VoidEvent _scanSuccess;
    [SerializeField] private GuestEvent _guestUpdatedSubscription;
    

    async void Start()
    {
        _rawImage.texture = await P4PGraphqlManager.Instance.GetQrLink();
        _timer.SetDuration(60).Begin();
    }

    private void OnEnable()
    {
        _guestUpdatedSubscription.Register(ShowInfo);
    }

    private void OnDisable()
    {
        _guestUpdatedSubscription.Unregister(ShowInfo);
    }

    private void ShowInfo(Guest guest) {
        string showText = guest.email != null ? "Voucher has been sent to your email: " + ConvertEmail(guest.email) + ". Please check your email to receive voucher" : "Voucher has been sent to your phone number: " + FormatPhoneNumber(guest.phone) + ". Please check your SMS to receive voucher";
        _textShow.text = showText;
        _scanSuccess.Raise();
        PlayAnimation();
        WarningTimerAnimation();
    }

    private void PlayAnimation()
    {
        Debug.Log("PlayAnimation");
        _voucherAnimator.SetBool("FadeOut", true);
        _qrAnimator.SetBool("ZoomIn", true);
        _notifyAnimator.SetBool("FadeIn", true);
        _notifyAnimator.SetBool("Male", UdpSocket.Instance.DataReceived.Gender < 0.5 ? true : false);
    }

    private void WarningTimerAnimation()
    {
        Debug.Log("Warning");
        _warningAnimator.SetBool("Warning", true);
    }

    private string ConvertEmail(string email){
        var parts = email.Split('@');
        var localPart = parts[0];
        var domainPart = parts[1];

        // Determine the number of characters to keep and mask
        int keepLength = Math.Min(3, localPart.Length);
        int maskedLength = localPart.Length - keepLength;

        // Create the masked local part
        var maskedLocalPart = localPart.Substring(0, keepLength) + new string('*', maskedLength);

        // Combine the masked local part with the domain part
        var maskedEmail = maskedLocalPart + "@" + domainPart;

        return maskedEmail;
    }

    private string FormatPhoneNumber(string phoneNumber)
    {
        // Remove the country code from the phone number
        string localNumber = phoneNumber.Substring(3);

        // Validate the length of the local number
        if (localNumber.Length >= 7) // Minimum 7 digits required
        {
            // Retain the first digit
            char firstDigit = localNumber[0];

            // Retain the last three digits
            string lastThreeDigits = localNumber.Substring(localNumber.Length - 3);

            // Construct the formatted number
            string formattedNumber = $"(+84) {firstDigit}** *** {lastThreeDigits}";

            return formattedNumber;
        }
        else
        {
            Debug.LogError("Invalid local number length.");
            return null;
        }
    }
}
