using Cysharp.Threading.Tasks;

namespace Services.PopUp
{
    public interface IPopUpService
    {
        UniTask<IPopUp> Open<T>(PopUpModel popUpModel = null) where T : IPopUp, new();
    }
}