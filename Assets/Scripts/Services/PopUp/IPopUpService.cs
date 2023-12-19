using Cysharp.Threading.Tasks;

namespace Services.PopUp
{
    public interface IPopUpService
    {
        UniTask<IPopUp> Open<T>(PopUpModel popUpModel) where T : IPopUp, new();
    }
}