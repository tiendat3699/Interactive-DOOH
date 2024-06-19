using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Timer UI references: ")]
    [SerializeField] private Image _uiFillImage;
    [SerializeField] private Text _uiText;
    public UnityEvent WarningTimer; 

    public int Duration { get; private set; }

    private int _remainingDuration;
    private bool _playAnimation = false;

    private void Awake()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        _uiText.text = "00:00";
        _uiFillImage.fillAmount = 0f;

        Duration = _remainingDuration = 0;
    }

    public Timer SetDuration (int seconds)
    {
        Duration = _remainingDuration = seconds;
        return this;
    }

    public void Begin()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_remainingDuration > 0)
        {
            UpdateUI(_remainingDuration);
            _remainingDuration--;
            if(_remainingDuration < 30 && !_playAnimation)
            {
                WarningTimer.Invoke();
                _playAnimation = true;
            }
            yield return new WaitForSeconds(1f);
        }

        End();
    }

    private void UpdateUI(int seconds)
    {
        _uiText.text = string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
        _uiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
    }

    public void End()
    {
        ResetTimer();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
