using Cysharp.Threading.Tasks;
using Modules.Widget.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Services.Widget
{
    public class WidgetService : IWidgetService
    {
        private const string CanvasKey = "CanvasPrefab";

        private readonly DiContainer _container;

        private GameObject _widgetCanvasInstance;
        private ILivesWidgetController _livesWidgetController;

        public WidgetService(DiContainer container)
        {
            _container = container;
            AddWidget().Forget();
        }

        private async UniTask AddWidget()
        {
            await InstantiateCanvas();

            _livesWidgetController = new LivesWidgetController();
            _container.Inject(_livesWidgetController);

            var widgetInstance = await InstantiateWidget(_livesWidgetController.WidgetKey);

            _livesWidgetController.Init(widgetInstance);
            _livesWidgetController.Show();
        }

        private async UniTask<GameObject> InstantiateWidget(string widgetKey)
        {
            var prefab = await Addressables.LoadAssetAsync<GameObject>(widgetKey);

            return _container.InstantiatePrefab(prefab, _widgetCanvasInstance.transform);
        }

        private async UniTask InstantiateCanvas()
        {
            var prefab = await Addressables.LoadAssetAsync<GameObject>(CanvasKey);
            _widgetCanvasInstance = _container.InstantiatePrefab(prefab);

            Object.DontDestroyOnLoad(_widgetCanvasInstance);
        }
    }
}