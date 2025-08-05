namespace MiniStop.Service.Interface
{
    public interface IPassword
    {
        string HashPassword(string password);

        string GenFirstPassword(int length = 16);
    }
}
