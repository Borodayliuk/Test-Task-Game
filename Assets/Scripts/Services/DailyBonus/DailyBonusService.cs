using System;
using Cysharp.Threading.Tasks;
using Modules.DailyBonusPopUp.Scripts;
using Services.PopUp;
using Services.User;

namespace Services.DailyBonus
{
    public class DailyBonusService : IDailyBonusService
    {
        private readonly IUserService _userService;
        private readonly IPopUpService _popUpService;

        public DailyBonusService(
            IUserService userService,
            IPopUpService popUpService)
        {
            _userService = userService;
            _popUpService = popUpService;

            AccrualDailyBonus().Forget();
        }

        private async UniTask AccrualDailyBonus()
        {
            var elapsedDays = (DateTime.Now - _userService.LastDailyBonusReceivedTime).Days;

            if (elapsedDays == 0)
                return;

            await _popUpService.Open<DailyBonusPopUp>(new PopUpModel
            {
                BonusAmount = GetBonusAmount()
            });

            _userService.SetLastDailyBonusReceivedTime(DateTime.Now);
        }

        private int GetBonusAmount()
        {
            var dayInSeason = UtilityDate.GetDayInSeason(DateTime.Now);
            return CalculateCoins(dayInSeason);
        }

        private int CalculateCoins(int dayInSeason)
        {
            switch (dayInSeason)
            {
                case 1:
                    return 2;
                case 2:
                    return 3;
                default:
                {
                    var coinsFromPreviousDay = CalculateCoins(dayInSeason - 1);
                    var coinsFromDayBefore = CalculateCoins(dayInSeason - 2);
                    var currentDayCoins = (int)(0.6 * coinsFromDayBefore + coinsFromPreviousDay);

                    return currentDayCoins;
                }
            }
        }
    }
}