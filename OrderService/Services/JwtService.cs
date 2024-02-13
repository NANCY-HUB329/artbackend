namespace Authentications.Services
{
    using Authentications.Utilities;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace AuthService.Services
    {
        public class JwtService 
        {
            private readonly JwtOptions _jwtOptions;
            public JwtService(IOptions<JwtOptions> options)
            {
                _jwtOptions = options.Value;
            }

            internal string? GetEmailFromToken(string jwtToken)
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadToken(processToken(jwtToken)) as JwtSecurityToken;

                var userId = token?.Claims.FirstOrDefault(claim => claim.Type == "email");

                return userId?.Value;
            }

            internal String processToken(String rawToken)
            {

                String[] tokenArray = rawToken.Split(" ");
                String token = tokenArray[1];
                return token;


            }

        }
    }
}
