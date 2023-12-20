using System;

namespace Services.User
{
    public interface IUserService
    {
        event Action<int> LivesAmountChanged;
        int LivesAmount { get; }
        DateTime LastLivesRefillTime { get; }
        DateTime LastDailyBonusReceivedTime { get; }
        void SetLives(int amount);
        void AddLives(int amount);
        void SetLastLivesRefillTime(DateTime dateTime);
        void SetLastDailyBonusReceivedTime(DateTime dateTime);
        void SubtractLives(int amount);
    }
}