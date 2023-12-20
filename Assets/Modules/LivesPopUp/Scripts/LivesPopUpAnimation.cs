using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.LivesPopUp.Scripts
{
    public class LivesPopUpAnimation : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private float animationDelay;
        [SerializeField] private float startXPosition;
        [SerializeField] private float showXPosition;

        public async UniTask ShowAnimation()
        {
        }

        public async UniTask HideAnimation()
        {
            
        }
    }
}