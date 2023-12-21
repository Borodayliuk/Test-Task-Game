using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.LivesPopUp.Scripts
{
    public class LivesPopUpAnimation : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private RectTransform popUpBodyRectTransform;
        [SerializeField] private float animationDelay;
        [SerializeField] private float startAnimationPositionX;
        [SerializeField] private float fadeValue;

        private Vector3 _showPosition;

        private void Awake()
        {
            _showPosition = popUpBodyRectTransform.position;
        }

        public async UniTask ShowAnimation()
        {
            popUpBodyRectTransform.position = new Vector3(startAnimationPositionX, _showPosition.y, _showPosition.z);

            gameObject.SetActive(true);
            popUpBodyRectTransform.DOMoveX(_showPosition.x, animationDelay).SetEase(Ease.Linear);
            await backgroundImage.DOFade(fadeValue, animationDelay).AsyncWaitForCompletion();
        }

        public async UniTask HideAnimation()
        {
            popUpBodyRectTransform.DOMoveX(startAnimationPositionX, animationDelay);
            await backgroundImage.DOFade(0, animationDelay).AsyncWaitForCompletion();
        }
    }
}