using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.PopUps.Scripts
{
    public abstract class PopupAssetPackage : MonoBehaviour
    {
        public abstract UniTask Init();
        public abstract UniTask Open();
        public abstract UniTask Close();
    }
}