using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services.PopUp
{
    public interface IPopUp
    {
        string PopUpKey { get; }
        UniTask Init(GameObject popUpInstance);
        UniTask Open();
        UniTask Close();
    }
}