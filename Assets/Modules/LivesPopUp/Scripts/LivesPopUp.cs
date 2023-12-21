using Cysharp.Threading.Tasks;
using Services.LivesRefill;
using Services.PopUp;
using Services.User;
using UnityEngine;
using Zenject;

namespace Modules.LivesPopUp.Scripts
{
    public class LivesPopUp : IPopUp
    {
        private LivesPopUpAssetPackage _assetPackage;
        private LivesPopUpContentType _actualContentType;
        private LivesPopUpContent _actualPopUpContent;
        private IUserService _userService;
        private ILivesRefillService _refillService;

        public string PopUpKey => nameof(LivesPopUp);

        [Inject]
        public void Construct(
            IUserService userService,
            ILivesRefillService refillService)
        {
            _userService = userService;
            _refillService = refillService;
        }

        public UniTask Init(GameObject popUpInstance, PopUpModel popUpModel)
        {
            _assetPackage = popUpInstance.GetComponent<LivesPopUpAssetPackage>();
            _assetPackage.Init();

            return UniTask.CompletedTask;
        }

        public async UniTask Open()
        {
            _actualContentType = GetPopUpContentType(_userService.LivesAmount);
            _actualPopUpContent = _assetPackage.ChangeContent(_actualContentType);

            _actualPopUpContent.SetLivesCountText(_userService.LivesAmount.ToString());
            await _assetPackage.Open();
            _assetPackage.SetTimerTextGetter(() => _refillService.GetTimeUntilNextRefill());

            SubscribeEvents();
            SubscribeContentEvents(_actualPopUpContent);
        }

        public async UniTask Close()
        {
            await _assetPackage.Close();
            UnsubscribeEvents();

            if (_actualPopUpContent != null)
                UnsubscribeContentEvents(_actualPopUpContent);
        }

        private LivesPopUpContentType GetPopUpContentType(int livesAmount)
        {
            return livesAmount switch
            {
                0 => LivesPopUpContentType.Empty,
                5 => LivesPopUpContentType.Full,
                _ => LivesPopUpContentType.NotFull
            };
        }

        private void SubscribeEvents()
        {
            _assetPackage.CloseButtonClicked += OnCloseClicked;
            _userService.LivesAmountChanged += OnLivesAmountChanged;
        }

        private void UnsubscribeEvents()
        {
            _assetPackage.CloseButtonClicked -= OnCloseClicked;
            _userService.LivesAmountChanged -= OnLivesAmountChanged;
        }

        private void OnCloseClicked()
            => Close().Forget();

        private void OnLivesAmountChanged(int amount)
        {
            ChangeContent(amount);
            _actualPopUpContent.SetLivesCountText(amount.ToString());
        }

        private void ChangeContent(int livesAmount)
        {
            var actualContentType = GetPopUpContentType(livesAmount);

            if (actualContentType == _actualContentType)
                return;

            UnsubscribeContentEvents(_actualPopUpContent);

            _actualContentType = actualContentType;
            _actualPopUpContent = _assetPackage.ChangeContent(actualContentType);

            SubscribeContentEvents(_actualPopUpContent);
        }

        private void SubscribeContentEvents(LivesPopUpContent content)
        {
            content.RefillButtonClicked += OnRefillButtonClicked;
            content.UseLifeButtonClicked += OnUseLifeButtonClicked;
        }

        private void UnsubscribeContentEvents(LivesPopUpContent content)
        {
            content.RefillButtonClicked -= OnRefillButtonClicked;
            content.UseLifeButtonClicked -= OnUseLifeButtonClicked;
        }

        private void OnRefillButtonClicked()
            => _userService.SetLives(Constants.MaxLives);

        private void OnUseLifeButtonClicked()
            => _userService.SubtractLives(1);
    }
}