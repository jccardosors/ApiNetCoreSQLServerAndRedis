using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Interfaces;
using Project.Application.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Application.Servicos
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ILogsService _logsService;
        private readonly IHttpContextAccessor _http;

        public TokenService(IConfiguration config, ILogsService logsService, IHttpContextAccessor http)
        {
            _config = config;
            _logsService = logsService;
            _http = http;
        }

        public string GenerateToken(string email)
        {
            var tokenString = string.Empty;
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, email));

            var key = _config["Jwt:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var tokenDuration = Convert.ToInt32(_config["Jwt:TokenDuration"]);
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenDuration),
                signingCredentials: signinCredentials);

            tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public async Task<CustomResult<string>> GetEmailbyTokenClaims()
        {
            string email = string.Empty;
            try
            {
                var identity = _http.HttpContext.User.Identity! as ClaimsIdentity;

                IEnumerable<Claim> claim = identity.Claims;

                var usernameClaim = claim
                    .Where(x => x.Type == ClaimTypes.Name)
                    .FirstOrDefault();

                email = usernameClaim!.Value;

                await _logsService.Add(email, "TokenService", "GetEmailbyTokenClaims", string.Empty);

                return CustomResult<string>.Success(email);
            }
            catch (Exception ex)
            {
                await _logsService.Add(email, "TokenService", "GetEmailbyTokenClaims", ex.Message);
                return CustomResult<string>.Failure(CustomError.ExceptionError(ex.Message));
            }
        }
    }
}
