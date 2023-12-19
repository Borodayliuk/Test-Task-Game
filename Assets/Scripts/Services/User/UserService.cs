using Models;

namespace Services.User
{
    public class UserService : IUserService
    {

        public UserService(UserModel userModel)
        {
            
        }

        public void AddHearts(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void SubtractHearts(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void SavePlayerState()
        {
            
        }

        private void LoadPlayerState(UserModel userModel)
        {
            
        }
    }
}