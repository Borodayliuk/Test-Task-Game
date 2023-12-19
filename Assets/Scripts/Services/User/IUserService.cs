namespace Services.User
{
    public interface IUserService
    {
        void AddHearts(int amount);
        void SubtractHearts(int amount);
        void SavePlayerState();
    }
}