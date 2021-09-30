namespace ComplaintsAdmin.Model
{
    public class AdminUser : IAdminUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }
}
