using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Services.PopUp
{
    public class PopUpService : IPopUpService
    {
        private const string PopUpsCanvasKey = "PopUpsCanvas";

        private readonly Dictionary<string, IPopUp> _popUps = new ();

        private GameObject _popUpsCanvasInstance;

        public PopUpService()
        {
            InstantiatePopUpsCanvas();
        }

        public async UniTask<IPopUp> Open<T>(PopUpModel popUpModel) where T : IPopUp, new()
        {
            if (_popUpsCanvasInstance == null)
                await InstantiatePopUpsCanvas();

            T popUp = new ();
            var popUpKey = popUp.PopUpKey;

            if (_popUps.TryGetValue(popUpKey, out var existingPopUp))
            {
                await existingPopUp.Open();
                return existingPopUp;
            }

            var popUpInstance = await InstantiatePopUp(popUpKey);

            await InitPopup(popUp, popUpInstance);
            await popUp.Open();
            return popUp;
        }

        private async UniTask<GameObject> InstantiatePopUp(string popUpKey)
        {
            var prefab = await Addressables.LoadAssetAsync<GameObject>(popUpKey);
            return Object.Instantiate(prefab, _popUpsCanvasInstance.transform);
        }

        private async UniTask InitPopup(IPopUp popUp, GameObject popUpInstance)
        {
            await popUp.Init(popUpInstance);

            _popUps.Add(popUp.PopUpKey, popUp);
        }

        private async UniTask InstantiatePopUpsCanvas()
        {
            var prefab = await Addressables.LoadAssetAsync<GameObject>(PopUpsCanvasKey);
            _popUpsCanvasInstance = Object.Instantiate(prefab);

            Object.DontDestroyOnLoad(_popUpsCanvasInstance);
        }
    }
}