using Cysharp.Threading.Tasks;

namespace Services.PopUp
{
    public interface IPopUpService
    {
        UniTask<IPopUp> Open<T>() where T : IPopUp, new();
    }
}