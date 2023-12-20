using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.PopUps.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.LivesPopUp.Scripts
{
    public class LivesPopUpAssetPackage : PopupAssetPackage
    {
        public Action CloseButtonClicked;

        [SerializeField] private LivesPopUpAnimation livesPopUpAnimation;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button closeArea;
        [SerializeField] private LivesPopUpContent contentsFull;
        [SerializeField] private LivesPopUpContent contentsNotFull;
        [SerializeField] private LivesPopUpContent contentsEmpty;

        private Dictionary<LivesPopUpContentType, LivesPopUpContent> _contentDictionary;
        private LivesPopUpContent _actualContent;
        private IEnumerator _timerTextRefreshingRoutine;
        private bool _isTimerTextRefreshing;

        private void OnEnable()
        {
            if (_timerTextRefreshingRoutine != null && _isTimerTextRefreshing)
                StartCoroutine(_timerTextRefreshingRoutine);
        }

        public override UniTask Init()
        {
            livesPopUpAnimation.gameObject.SetActive(false);

            _contentDictionary = new Dictionary<LivesPopUpContentType, LivesPopUpContent>
            {
                [LivesPopUpContentType.Full] = contentsFull,
                [LivesPopUpContentType.NotFull] = contentsNotFull,
                [LivesPopUpContentType.Empty] = contentsEmpty,
            };

            return UniTask.CompletedTask;
        }

        public override async UniTask Open()
        {
            await livesPopUpAnimation.ShowAnimation();
            SubscribeEvents();
        }

        public override async UniTask Close()
        {
            await livesPopUpAnimation.HideAnimation();
            livesPopUpAnimation.gameObject.SetActive(false);
        }

        public LivesPopUpContent ChangeContent(LivesPopUpContentType type)
        {
            if (!_contentDictionary.TryGetValue(type, out var actualContent))
                return _actualContent;

            if (actualContent.IsActive)
                return _actualContent;

            HideAllContent();

            _actualContent = actualContent;
            _actualContent.gameObject.SetActive(true);
            return actualContent;
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

        private void HideAllContent()
        {
            foreach (var content in _contentDictionary.Values)
                content.gameObject.SetActive(false);
        }

        private void SubscribeEvents()
        {
            closeButton.onClick.AddListener(OnCloseClicked);
            closeArea.onClick.AddListener(OnCloseClicked);
        }

        private void UnsubscribeEvents()
        {
            closeButton.onClick.RemoveListener(OnCloseClicked);
            closeArea.onClick.RemoveListener(OnCloseClicked);
        }

        private void OnCloseClicked()
        {
            UnsubscribeEvents();
            CloseButtonClicked?.Invoke();
        }

        private IEnumerator TimerLabelRefreshingRoutine(Func<string> timeLeftTextGetter, float refreshRateInSeconds = 0.1f)
        {
            if (timeLeftTextGetter == null)
                yield return null;
            
            _isTimerTextRefreshing = true;
            while (_isTimerTextRefreshing)
            {
                foreach (var content in _contentDictionary.Values)
                    content.SetTimerText(timeLeftTextGetter?.Invoke());

                yield return new WaitForSecondsRealtime(refreshRateInSeconds);
            }
        }
    }
}