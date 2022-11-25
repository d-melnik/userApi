using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserApiDbClient.Entities;

namespace UserApiServices.Helpers
{
    public interface IJwtUtils
    {
        public string GenerateToken(UserEntity user);
    }

    public class JwtUtils : IJwtUtils
    {
        private IConfiguration Configuration { get; }

        public JwtUtils(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateToken(UserEntity user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["JWT:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(BuildUserClaims(user)),
                
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private IEnumerable<Claim> BuildUserClaims(UserEntity user)
        {
            IList<Claim> userClaims = user.UserClaim.Select(c => new Claim(ClaimTypes.Role, c.Claim.Name)).ToList();
            userClaims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email));
            userClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            return userClaims;
        }
    }
}