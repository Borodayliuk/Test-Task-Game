using System;
using Services.LivesRefill;
using Services.PopUp;
using Services.User;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Modules.Widget.Scripts
{
    public class LivesWidgetController : ILivesWidgetController, IDisposable
    {
        private LivesWidgetView _livesWidgetView;
        private IUserService _userService;
        private IPopUpService _popUpService;
        private ILivesRefillService _refillService;

        public string WidgetKey => "LivesWidget";

        [Inject]
        public void Construct(
            IUserService userService,
            IPopUpService popUpService,
            ILivesRefillService refillService)
        {
            _userService = userService;
            _popUpService = popUpService;
            _refillService = refillService;
        }

        public void Init(GameObject widget)
        {
            _livesWidgetView = widget.GetComponent<LivesWidgetView>();
            _livesWidgetView.Init();

            widget.SetActive(false);
        }

        public void Show()
        {
            OnLivesAmountChanged(_userService.LivesAmount);

            _livesWidgetView.gameObject.SetActive(true);
            _livesWidgetView.SetTimerTextGetter(() => _refillService.GetTimeUntilNextRefill());

            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
            Object.Destroy(_livesWidgetView.gameObject);
        }

        private void SubscribeEvents()
        {
            _livesWidgetView.ButtonClicked += OnWidgetClicked;
            _userService.LivesAmountChanged += OnLivesAmountChanged;
        }

        private void UnsubscribeEvents()
        {
            _livesWidgetView.ButtonClicked -= OnWidgetClicked;
            _userService.LivesAmountChanged -= OnLivesAmountChanged;
        }

        private void OnWidgetClicked()
        {
            _popUpService.Open<LivesPopUp.Scripts.LivesPopUp>();
        }

        private void OnLivesAmountChanged(int amount)
        {
            _livesWidgetView.ChangeTimerText(isFullLives: amount == Constants.MaxLives);
            _livesWidgetView.SetLivesAmountText(_userService.LivesAmount.ToString());
        }
    }
}