using UnityEngine;

namespace Modules.Widget.Scripts
{
    public interface ILivesWidgetController
    {
        string WidgetKey { get; }
        void Init(GameObject widget);
        void Show();
    }
}