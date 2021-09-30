namespace ComplaintsAdmin.Model
{
    public interface IAdminUser
    {
        string Login { get; set; }
        string Password { get; set; }
        string Status { get; set; }
    }
}