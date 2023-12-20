using System;
using Models;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly UserModel _userModel = new();

        public int LivesAmount => _userModel.LivesAmount;
        public DateTime LastLivesRefillTime => _userModel.LastRefillTime;

        public UserService()
        {
            
        }

        public void SetLives(int amount)
        {
            if (amount is < 0 or > Constants.MaxLives)
                return;

            _userModel.LivesAmount = amount;

            GlobalGameEvents.LivesAmountChanged?.Invoke(_userModel.LivesAmount);
        }

        public void AddLives(int amount)
        {
            if (_userModel.LivesAmount + amount > Constants.MaxLives)
                return;

            _userModel.LivesAmount += amount;

            GlobalGameEvents.LivesAmountChanged?.Invoke(_userModel.LivesAmount);
        }

        public void SubtractLives(int amount)
        {
            if (_userModel.LivesAmount - amount < 0)
                return;

            _userModel.LivesAmount -= amount;

            GlobalGameEvents.LivesAmountChanged?.Invoke(_userModel.LivesAmount);
        }

        public void SetLastLivesRefillTime(DateTime dateTime)
            => _userModel.LastRefillTime = dateTime;

        public void SavePlayerState()
        {
            
        }

        private void LoadPlayerState(UserModel userModel)
        {
        }
    }
}