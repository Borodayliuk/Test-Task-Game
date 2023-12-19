using Services.PopUp;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallService();
        }

        private void InstallService()
        {
            Container.Bind<IPopUpService>().To<PopUpService>().AsSingle().NonLazy();
        }
    }
}
