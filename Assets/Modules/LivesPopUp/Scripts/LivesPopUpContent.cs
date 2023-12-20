using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.LivesPopUp.Scripts
{
    public class LivesPopUpContent : MonoBehaviour
    {
        public Action UseLifeButtonClicked;
        public Action RefillButtonClicked;

        [SerializeField] private Button useLifeButton;
        [SerializeField] private Button refillButton;
        [SerializeField] private TextMeshProUGUI livesCountText;
        [SerializeField] private TextMeshProUGUI timerText;

        public bool IsActive => gameObject.activeSelf;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void SetLivesCountText(string text)
            => livesCountText.text = text;

        public void SetTimerText(string text)
            => timerText.text = text;

        private void OnRefillClicked()
            => RefillButtonClicked?.Invoke();

        private void OnUseLifeClicked()
            => UseLifeButtonClicked?.Invoke();

        private void SubscribeEvents()
        {
            useLifeButton.onClick.AddListener(OnUseLifeClicked);
            refillButton.onClick.AddListener(OnRefillClicked);
        }

        private void UnsubscribeEvents()
        {
            useLifeButton.onClick.RemoveListener(OnUseLifeClicked);
            refillButton.onClick.RemoveListener(OnRefillClicked);
        }
    }
}
