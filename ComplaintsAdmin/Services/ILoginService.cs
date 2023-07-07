namespace ComplaintsAdmin.Services
{
    public interface ILoginService
    {
        bool Authenticate(string userName, string password);
    }
}
