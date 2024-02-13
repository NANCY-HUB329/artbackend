namespace Authentications.Services.Iservices
{
    using Authentications.Models.AuthService.Models;
    using Authentications.Models.Dtos;

    namespace AuthService.Services.IServices
    {
        public interface IJwt
        {

            string GenerateToken(ApplicationUser user, IEnumerable<string> Roles);
        }
    }

}
