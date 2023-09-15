using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public  IConfiguration _Configuration { get; }

        public class AuthenticationRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
        
        private class CityInfoUser
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public CityInfoUser(int userId,
                string username, 
                string firstName,
                string lastName, 
                string city)
            {
                UserId = userId;
                Username = username;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }
        public AuthenticationController(IConfiguration configuration)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {   
            // Step 1: Validate the username/password
            var user = ValidateUserCredentials(
                authenticationRequestBody.Username,
                authenticationRequestBody.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            // Step 2: Create a tokem
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_Configuration["Authentication:SecretForKey"]));
            var signingCredentials =new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                _Configuration["Authentication:Issuer"],
                _Configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private CityInfoUser ValidateUserCredentials(string? username, string? password)
        {   
            // For demo purpose assume the credentials are valid
            return new CityInfoUser(
                1,
                username ?? "",
                "kevin",
                "Dockx",
                "Antwerp");
        }
    }
}
