using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BCrypt.Net;

namespace RaFilDaAPI.Entities
{
    public class AuthenticationService
    {
        public const string SECRET = "foo-bar";

        private readonly MyContext myContext; 

        public AuthenticationService(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public string Authenticate(Credentials credentials)
        {
            try
            {
                if(myContext.Users.ToList().Count == 0 && credentials.Login == "admin" && credentials.Password == "admin")
                    return JwtBuilder.Create()
                        .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                        .WithSecret(SECRET)
                        .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(3600).ToUnixTimeSeconds())
                        .AddClaim("role", "admin")
                        .Encode();
                
                User user = this.myContext.Users.ToList().FirstOrDefault(x => x.Username == credentials.Login &&
                                                                            BCrypt.Net.BCrypt.Verify(credentials.Password, x.Password, false, HashType.SHA384));

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
                HttpClient http = new HttpClient(new HttpClientHandler() { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator });;
                Task<string> result = http.PostAsync("https://localhost:5001/api/Sessions/banned?token=" + token, null).Result.Content.ReadAsStringAsync();
                if (Boolean.Parse(result.Result))
                    return "";
                
                string json = JwtBuilder.Create()
                             .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                             .WithSecret(SECRET)
                             .MustVerifySignature()
                             .Decode(token);

                return json;
            }
            catch
            {
                return "";
            }
        }
    }
}
