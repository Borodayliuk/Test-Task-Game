using Services.DailyBonus;
using Services.LivesRefill;
using Services.PopUp;
using Services.User;
using Services.Widget;
using Zenject;

namespace Installers
{
    public class Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallServices();
        }

        private void InstallServices()
        {
            Container.Bind<IUserService>().To<UserService>().AsSingle();
            Container.Bind(typeof(ITimerService), typeof(ITickable)).To<TimerService>().AsSingle();
            Container.Bind<ILivesRefillService>().To<LivesRefillService>().AsSingle().NonLazy();
            Container.Bind<IPopUpService>().To<PopUpService>().AsSingle();
            Container.Bind<IDailyBonusService>().To<DailyBonusService>().AsSingle().NonLazy();
            Container.Bind<IWidgetService>().To<WidgetService>().AsSingle().NonLazy();
        }
    }
}
