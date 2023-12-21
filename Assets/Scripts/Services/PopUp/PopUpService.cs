using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Services.PopUp
{
    public class PopUpService : IPopUpService
    {
        private const string CanvasKey = "CanvasPrefab";

        private readonly DiContainer _container;
        private readonly Dictionary<string, IPopUp> _popUps = new Dictionary<string, IPopUp>();

        private GameObject _popUpsCanvasInstance;

        public PopUpService(DiContainer container)
        {
            _container = container;
        }

        public async UniTask<IPopUp> Open<T>(PopUpModel popUpModel) where T : IPopUp, new()
        {
            if (_popUpsCanvasInstance == null)
                await InstantiatePopUpsCanvas();

            var popUp = new T();
            var popUpKey = popUp.PopUpKey;

            if (_popUps.TryGetValue(popUpKey, out var existingPopUp))
            {
                await existingPopUp.Open();
                return existingPopUp;
            }

            var popUpInstance = await InstantiatePopUp(popUpKey);

            _container.Inject(popUp);

            await InitPopup(popUp, popUpInstance, popUpModel);
            await popUp.Open();
            return popUp;
        }

        private async UniTask<GameObject> InstantiatePopUp(string popUpKey)
        {
            var prefab = await Addressables.LoadAssetAsync<GameObject>(popUpKey);
            return _container.InstantiatePrefab(prefab, _popUpsCanvasInstance.transform);
        }

        private async UniTask InitPopup(IPopUp popUp, GameObject popUpInstance, PopUpModel popUpModel)
        {
            await popUp.Init(popUpInstance, popUpModel);

            _popUps.Add(popUp.PopUpKey, popUp);
        }

        private async UniTask InstantiatePopUpsCanvas()
        {
            var prefab = await Addressables.LoadAssetAsync<GameObject>(CanvasKey);
            _popUpsCanvasInstance = _container.InstantiatePrefab(prefab);

            Object.DontDestroyOnLoad(_popUpsCanvasInstance);
        }
    }
}