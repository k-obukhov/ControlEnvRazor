using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlEnvRazor
{
    public class AuthOptions
    {
        public const string ISSUER = "ControlEnvRazor"; // издатель токена
        public const string AUDIENCE = "http://localhost:45138"; // потребитель токена
        const string KEY = "secretsecretsecretsecretsecretsecretsecret";   // ключ для шифрации
        public const int LIFETIME = 5; // время жизни токена 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
