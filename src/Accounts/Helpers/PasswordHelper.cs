using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Helpers
{
    public static class PasswordHelper
    {
        public static bool PasswordsMatch(string givenPassword, string storedPassword)
        {
            using (var hashBuilder = SHA512.Create())
            {
                byte[] bytePassword = hashBuilder.ComputeHash(Encoding.UTF8.GetBytes(givenPassword));
                var base64Password = Convert.ToBase64String(bytePassword);

                if (base64Password != storedPassword)
                {
                    return false;
                }
            }

            return true;
        }

        public static string HashPassword(string password)
        {
            using (var hashBuilder = SHA512.Create())
            {
                byte[] bytePassword = hashBuilder.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytePassword);
            }
        }
    }
}
