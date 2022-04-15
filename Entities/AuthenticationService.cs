using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaFilDaAPI.Entities
{
    public class AuthenticationService
    {
        const string SECRET = "foo-bar";

        private readonly MyContext context; // null takže to nejde

        /*public AuthenticationService(MyContext myContext)
        {
            this.context = context;
        }*/

        public string Authenticate(Credentials credentials)
        {
            User user = this.context.Users.Where(x => x.Username == credentials.Login && x.Password == credentials.Password).FirstOrDefault();

            if (user == null)
                throw new Exception("invalid user");

            return JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(SECRET)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(30).ToUnixTimeSeconds())
                      .AddClaim("user_id", user.Id)
                      .Encode();
        }

        public bool VerifyToken(string token)
        {
            try
            {
                string json = JwtBuilder.Create()
                             .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                             .WithSecret(SECRET)
                             .MustVerifySignature()
                             .Decode(token);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
