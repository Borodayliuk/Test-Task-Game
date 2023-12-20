using System;
using ModestTree;
using Newtonsoft.Json;
using UnityEngine;
using UserData;

namespace Services.User
{
    public class UserService : IUserService
    {
        private const string UserModelKey = "UserModel";

        public event Action<int> LivesAmountChanged;

        private UserModel _userModel;

        public int LivesAmount => _userModel.LivesAmount;
        public DateTime LastLivesRefillTime => _userModel.LastRefillTime;
        public DateTime LastDailyBonusReceivedTime => _userModel.LastDailyBonusReceivedTime;

        public UserService()
        {
            //ClearPlayerPref();
            LoadPlayerState();
        }

        public void SetLives(int amount)
        {
            if (amount is < 0 or > Constants.MaxLives)
                return;

            _userModel.LivesAmount = amount;

            LivesAmountChanged?.Invoke(_userModel.LivesAmount);
            SavePlayerState();
        }

        public void AddLives(int amount)
        {
            if (_userModel.LivesAmount + amount > Constants.MaxLives)
                return;

            _userModel.LivesAmount += amount;
            LivesAmountChanged?.Invoke(_userModel.LivesAmount);
            SavePlayerState();
        }

        public void SubtractLives(int amount)
        {
            if (_userModel.LivesAmount - amount < 0)
                return;

            _userModel.LivesAmount -= amount;
            LivesAmountChanged?.Invoke(_userModel.LivesAmount);
            SavePlayerState();
        }

        public void SetLastLivesRefillTime(DateTime dateTime)
        {
            _userModel.LastRefillTime = dateTime;
            SavePlayerState();
        }

        public void SetLastDailyBonusReceivedTime(DateTime dateTime)
        {
            _userModel.LastDailyBonusReceivedTime = dateTime;
            SavePlayerState();
        }

        private void SavePlayerState()
        {
            var modelData = JsonConvert.SerializeObject(_userModel);
            PlayerPrefs.SetString(UserModelKey, modelData);
        }

        private void LoadPlayerState()
        {
            var loadedModelData = PlayerPrefs.GetString(UserModelKey);
            if (!loadedModelData.IsEmpty())
            {
                _userModel = new UserModel();
                _userModel = JsonConvert.DeserializeObject<UserModel>(loadedModelData);
                return;
            }

            _userModel = new UserModel();
            SavePlayerState();
        }

        // NOTE: Use to clear user data (testing only)
        private void ClearPlayerPref()
            => PlayerPrefs.DeleteKey(UserModelKey);
    }
}