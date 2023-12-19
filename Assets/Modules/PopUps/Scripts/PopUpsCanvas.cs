using UnityEngine;

namespace Modules.PopUps.Scripts
{
    public class PopUpsCanvas : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private void Awake()
        {
            SetupCanvas();
        }

        private void SetupCanvas()
            => canvas.worldCamera = Camera.main;
    }
}
