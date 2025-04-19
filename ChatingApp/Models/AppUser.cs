namespace ChatingApp.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}
