using ChatingApp.Models;

namespace ChatingApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);

    }
}
