using UnityEngine;

namespace UI
{
    public class CanvasUI : MonoBehaviour
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
