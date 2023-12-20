using System;

namespace UserData
{
    public class UserModel
    {
        public int LivesAmount { get; set; }
        public DateTime LastRefillTime { get; set; }
        public DateTime LastDailyBonusReceivedTime { get; set; }
    }
}