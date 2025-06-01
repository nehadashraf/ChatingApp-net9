using System.ComponentModel.DataAnnotations;

namespace ChatingApp.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }=string.Empty;
        [Required]
        public  string Password { get; set; }=string.Empty;
    }
}
