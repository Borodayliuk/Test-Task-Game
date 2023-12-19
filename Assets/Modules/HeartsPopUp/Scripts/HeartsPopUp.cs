using Cysharp.Threading.Tasks;
using Services.PopUp;
using UnityEngine;

namespace Modules.HeartsPopUp.Scripts
{
    public class HeartsPopUp : IPopUp
    {
        public string PopUpKey => nameof(HeartsPopUp);

        private HeartsPopUpAssetPackage _assetPackage;

        public UniTask Init(GameObject popUpInstance)
        {
            _assetPackage = popUpInstance.GetComponent<HeartsPopUpAssetPackage>();

            return UniTask.CompletedTask;
        }

        public async UniTask Open()
        {
            await _assetPackage.Open();
            SubscribeEvents();
        }

        public async UniTask Close()
        {
            await _assetPackage.Close();
            UnsubscribeEvents();
        }


        private void SubscribeEvents()
        {
            _assetPackage.CloseButtonClicked += OnCloseClicked;
        }

        private void UnsubscribeEvents()
        {
            _assetPackage.CloseButtonClicked -= OnCloseClicked;
        }

        private void OnCloseClicked()
            => Close().Forget();
    }
}