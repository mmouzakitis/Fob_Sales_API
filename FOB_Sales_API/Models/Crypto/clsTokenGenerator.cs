//using FOB_Sales_API.Models.KeyConstants;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Web.Http;
//using WebApi.Jwt;



namespace jwtTokens.Models.Crypto
{
    public class clsTokenGenerator
    {

        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
       // private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        //public static string GenerateToken(string username, int expireMinutes = 60)
        //{
        //    var symmetricKey = Convert.FromBase64String(Secret);
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    var now = DateTime.UtcNow;
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //                {
        //                new Claim(ClaimTypes.Name, username)
        //            }),

        //        Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var stoken = tokenHandler.CreateToken(tokenDescriptor);
        //    var token = tokenHandler.WriteToken(stoken);

        //    return token;
        //}


    }
}