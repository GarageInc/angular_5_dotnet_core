using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace depot {
    public class AuthOptions {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "http://localhost:4200/"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!1234"; // ключ для шифрации
        public const int LIFETIME = 60 * 24 * 14; // время жизни токена в минутах

        public const int LIFETIME_DAYS = AuthOptions.LIFETIME / 60 * 24;

        public static SymmetricSecurityKey GetSymmetricSecurityKey () {
            return new SymmetricSecurityKey (Encoding.ASCII.GetBytes (KEY));
        }
    }
}