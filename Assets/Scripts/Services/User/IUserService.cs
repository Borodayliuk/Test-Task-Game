using System;

namespace Services.User
{
    public interface IUserService
    {
        int LivesAmount { get; }
        DateTime LastLivesRefillTime { get; }
        void SetLives(int amount);
        void AddLives(int amount);
        void SetLastLivesRefillTime(DateTime dateTime);
        void SubtractLives(int amount);
        void SavePlayerState();
    }
}