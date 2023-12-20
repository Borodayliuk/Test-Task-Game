using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Widget.Scripts
{
    public class LivesWidgetView : MonoBehaviour
    {
        public Action ButtonClicked;

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI livesAmountText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject fullText;

        private IEnumerator _timerTextRefreshingRoutine;
        private bool _isTimerTextRefreshing;

        private void OnEnable()
        {
            if (_timerTextRefreshingRoutine != null && _isTimerTextRefreshing)
                StartCoroutine(_timerTextRefreshingRoutine);
        }

        public void Init()
            => button.onClick.AddListener(OnButtonClicked);

        public void SetLivesAmountText(string text)
            => livesAmountText.text = text;

        public void ChangeTimerText(bool isFullLives)
        {
            timerText.gameObject.SetActive(!isFullLives);
            fullText.SetActive(isFullLives);
        }

        public void SetTimerTextGetter(Func<string> timeLeftTextGetter)
        {
            if (timeLeftTextGetter == null)
            {
                StopCoroutine(nameof(TimerLabelRefreshingRoutine));
                _isTimerTextRefreshing = false;
                return;
            }

            if (_isTimerTextRefreshing)
                return;

            _timerTextRefreshingRoutine = TimerLabelRefreshingRoutine(timeLeftTextGetter);
            StartCoroutine(_timerTextRefreshingRoutine);
        }

        private void OnButtonClicked()
        {
            ButtonClicked?.Invoke();
        }

        private IEnumerator TimerLabelRefreshingRoutine(Func<string> timeLeftTextGetter, float refreshRateInSeconds = 0.1f)
        {
            if (timeLeftTextGetter == null)
                yield return null;
            
            _isTimerTextRefreshing = true;

            while (_isTimerTextRefreshing)
            {
                timerText.text = timeLeftTextGetter?.Invoke();

                yield return new WaitForSecondsRealtime(refreshRateInSeconds);
            }
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}
