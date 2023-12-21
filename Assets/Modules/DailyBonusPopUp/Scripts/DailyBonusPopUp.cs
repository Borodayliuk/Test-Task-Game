using Cysharp.Threading.Tasks;
using Services.PopUp;
using UnityEngine;

namespace Modules.DailyBonusPopUp.Scripts
{
    public class DailyBonusPopUp : IPopUp
    {
        private DailyBonusAssetPackage _dailyBonusAssetPackage;

        public string PopUpKey => nameof(DailyBonusPopUp);

        public UniTask Init(GameObject popUpInstance, PopUpModel popUpModel)
        {
            _dailyBonusAssetPackage = popUpInstance.GetComponent<DailyBonusAssetPackage>();
            _dailyBonusAssetPackage.Init();
            _dailyBonusAssetPackage.SetBonusText($"{popUpModel.BonusAmount} Coins");

            return UniTask.CompletedTask;
        }

        public UniTask Open()
        {
            _dailyBonusAssetPackage.Open();
            _dailyBonusAssetPackage.ClaimButtonClicked += OnClaimButtonClicked;

            return UniTask.CompletedTask;
        }

        public UniTask Close()
        {
            _dailyBonusAssetPackage.Close();
            _dailyBonusAssetPackage.ClaimButtonClicked += OnClaimButtonClicked;

            return UniTask.CompletedTask;
        }

        private void OnClaimButtonClicked()
            => Close();
    }
}