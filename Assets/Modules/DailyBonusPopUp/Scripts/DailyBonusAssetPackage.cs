using System;
using Cysharp.Threading.Tasks;
using Modules.PopUps.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.DailyBonusPopUp.Scripts
{
    public class DailyBonusAssetPackage : PopupAssetPackage
    {
        public Action OnClaimButtonClicked;

        [SerializeField] private Button claimButton;
        [SerializeField] private TextMeshProUGUI bonusText;

        public override UniTask Init()
        {
            gameObject.SetActive(false);

            return UniTask.CompletedTask;
        }

        public override UniTask Open()
        {
            gameObject.SetActive(true);
            claimButton.onClick.AddListener(ClaimClicked);

            return UniTask.CompletedTask;
        }

        public override UniTask Close()
        {
            claimButton.onClick.RemoveListener(ClaimClicked);
            gameObject.SetActive(false);

            return UniTask.CompletedTask;
        }

        public void SetBonusText(string text)
            => bonusText.text = text;

        private void ClaimClicked()
            => OnClaimButtonClicked?.Invoke();
    }
}