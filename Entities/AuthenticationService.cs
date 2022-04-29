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

        private readonly MyContext context; 

        public AuthenticationService(MyContext myContext)
        {
            this.context = myContext;
        }

        public string Authenticate(Credentials credentials)
        {
            try
            {
                User user = this.context.Users.Where(x => x.Username == credentials.Login && x.Password == credentials.Password).FirstOrDefault();

                if (user == null)
                    throw new UnauthorizedAccessException();
                
                return JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(SECRET)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(3600).ToUnixTimeSeconds())
                    .AddClaim("user_id", user.Id)
                    .AddClaim("role", "admin")
                    .Encode();
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException)
                    throw new Exception("Invalid username or password!");
                else
                    throw new Exception("Failed to connect to database!");
            }
        }

        public string VerifyToken(string token)
        {
            try
            {
                string json = JwtBuilder.Create()
                             .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                             .WithSecret(SECRET)
                             .MustVerifySignature()
                             .Decode(token);
                
                Console.WriteLine(JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(SECRET)
                    .AddClaim("role", "daemon")
                    .Encode());

                return json;
            }
            catch
            {
                return "";
            }
        }
    }
}
